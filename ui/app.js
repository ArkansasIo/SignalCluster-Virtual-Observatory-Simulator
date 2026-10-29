const $ = id => document.getElementById(id);
const API = "http://localhost:5080";
document.querySelectorAll("[data-page]").forEach(button => button.addEventListener("click", () => {
  document.querySelectorAll("main section").forEach(section => section.classList.toggle("active", section.id === button.dataset.page));
}));
async function api(path, options) {
  const response = await fetch(API + path, options);
  const body = await response.json();
  if (!response.ok) throw new Error(body.error || `HTTP ${response.status}`);
  return body;
}
function show(id, value) { $(id).textContent = typeof value === "string" ? value : JSON.stringify(value, null, 2); }
$("health").onclick = async () => { try { show("health-result", await api("/health")); } catch (e) { show("health-result", "API unavailable: " + e.message + ". Start the service at http://localhost:5080."); } };
$("radio-status").onclick = async () => { try { show("health-result", await api("/radio/status")); } catch (e) { show("health-result", e.message); } };
$("build-info").onclick = async () => { try { show("build-result", await api("/build-info")); } catch (e) { show("build-result", e.message); } };
$("fft").onclick = async () => {
  try {
    const samples = $("samples").value.split(",").map(value => Number(value.trim()));
    if (samples.some(value => !Number.isFinite(value))) throw new Error("Enter only finite numbers separated by commas.");
    show("fft-result", await api("/algorithms/fft-magnitude", { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify({ samples }) }));
  } catch (e) { show("fft-result", e.message); }
};
$("delay").onclick = async () => {
  try {
    show("delay-result", await api("/algorithms/dispersion-delay", { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify({ dispersionMeasure: Number($("dm").value), lowFrequencyMHz: Number($("low").value), highFrequencyMHz: Number($("high").value) }) }));
  } catch (e) { show("delay-result", e.message); }
};
$("plan-channels").onclick = async () => {
  try {
    const count = Number($("channel-count").value);
    if (!Number.isInteger(count)) throw new Error("Channel count must be a whole number.");
    const result = await api("/channels/plan", { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify({ startFrequencyMHz: Number($("channel-start").value), totalBandwidthMHz: Number($("channel-bandwidth").value), count, labelPrefix: $("channel-prefix").value }) });
    show("channel-result", { count: result.count, totalBandwidthMHz: result.totalBandwidthMHz, preview: result.channels.slice(0, 5), finalChannel: result.channels[result.channels.length - 1], note: "Showing the first five channels and the final channel; full plan is returned by the API." });
  } catch (e) { show("channel-result", e.message); }
};
$("generate-samples").onclick = async () => {
  try {
    const result = await api("/simulation/samples", { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify({ sampleRateHz: Number($("sample-rate").value), pulseRateHz: Number($("pulse-rate").value), toneFrequencyHz: Number($("sample-tone").value), durationSeconds: Number($("sample-duration").value), seed: Number($("sample-seed").value) }) });
    show("samples-result", { simulatedOnly: result.simulatedOnly, sampleRateHz: result.sampleRateHz, sampleCount: result.sampleCount, durationSeconds: result.durationSeconds, seed: result.seed, preview: result.samples.slice(0, 24), note: "Only the first 24 values are displayed here; the API response contains the full dataset." });
  } catch (e) { show("samples-result", e.message); }
};
let audioContext, source;
function generateAudio() {
  const sampleRate = 22050;
  const duration = Math.max(1, Math.min(30, Number($("duration").value) || 8));
  const length = sampleRate * duration;
  const data = new Float32Array(length);
  const frequency = Math.max(40, Math.min(12000, Number($("freq").value) || 700));
  const level = Math.max(0, Math.min(100, Number($("level").value) || 0)) / 100;
  const mode = $("mode").value;
  for (let i = 0; i < length; i++) {
    const t = i / sampleRate, phase = (t * 2) % 1;
    let value;
    if (mode === "pulsar") value = phase < .12 ? Math.sin(Math.PI * phase / .12) * Math.sin(2 * Math.PI * frequency * t) : 0;
    else if (mode === "am") value = (.5 + .5 * Math.sin(2 * Math.PI * 3 * t)) * Math.sin(2 * Math.PI * frequency * t);
    else if (mode === "noise") value = Math.random() * 2 - 1;
    else value = Math.sin(2 * Math.PI * frequency * t);
    const fade = Math.min(1, i / (sampleRate * .02), (length - i) / (sampleRate * .03));
    data[i] = value * level * Math.max(0, fade);
  }
  return { data, sampleRate };
}
function stopAudio() {
  try { source?.stop(); } catch {}
  source = null;
  if (audioContext) { const old = audioContext; audioContext = null; old.close().catch(() => {}); }
  if ($("radio-state")) $("radio-state").textContent = "Idle · simulated";
}
$("play").onclick = async () => {
  try {
    stopAudio();
    const generated = generateAudio();
    audioContext = new (window.AudioContext || window.webkitAudioContext)();
    if (audioContext.state === "suspended") await audioContext.resume();
    const buffer = audioContext.createBuffer(1, generated.data.length, generated.sampleRate);
    buffer.copyToChannel(generated.data, 0);
    source = audioContext.createBufferSource();
    source.buffer = buffer;
    source.connect(audioContext.destination);
    source.onended = () => { if (source) stopAudio(); };
    source.start();
    $("radio-state").textContent = "Playing synthetic audio";
  } catch (e) { $("radio-state").textContent = e.message; }
};
$("stop").onclick = stopAudio;
function makeWav(generated) {
  const { data, sampleRate } = generated;
  const buffer = new ArrayBuffer(44 + data.length * 2), view = new DataView(buffer);
  const writeText = (offset, text) => [...text].forEach((character, i) => view.setUint8(offset + i, character.charCodeAt(0)));
  writeText(0, "RIFF"); view.setUint32(4, 36 + data.length * 2, true); writeText(8, "WAVE");
  writeText(12, "fmt "); view.setUint32(16, 16, true); view.setUint16(20, 1, true); view.setUint16(22, 1, true);
  view.setUint32(24, sampleRate, true); view.setUint32(28, sampleRate * 2, true); view.setUint16(32, 2, true); view.setUint16(34, 16, true);
  writeText(36, "data"); view.setUint32(40, data.length * 2, true);
  for (let i = 0; i < data.length; i++) view.setInt16(44 + i * 2, Math.round(Math.max(-1, Math.min(1, data[i])) * 32767), true);
  return new Blob([buffer], { type: "audio/wav" });
}
$("wav").onclick = () => {
  const url = URL.createObjectURL(makeWav(generateAudio()));
  const link = document.createElement("a"); link.href = url; link.download = "signalcluster-synthetic-radio.wav"; link.click();
  setTimeout(() => URL.revokeObjectURL(url), 1000);
};
