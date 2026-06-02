// src/services/interviewApi.js
const BASE = 'https://localhost:57767/api/interview'

export async function startSession({ resume, jobDescription, candidateName }) {
  const res = await fetch(`${BASE}/start`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ resume, jobDescription, candidateName })
  })
  if (!res.ok) throw new Error(await res.text())
  return res.json() // { sessionId, firstQuestion, totalQuestions }
}

export async function submitAnswer({ sessionId, answer }) {
  const res = await fetch(`${BASE}/answer`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ sessionId, answer })
  })
  if (!res.ok) throw new Error(await res.text())
  return res.json() // { feedback, nextQuestion, questionNumber, totalQuestions, isComplete, scoreCard }
}

export async function abandonSession(sessionId) {
  await fetch(`${BASE}/${sessionId}`, { method: 'DELETE' })
}
