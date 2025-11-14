// src/services/api.js
import axios from 'axios'

const api = axios.create({
  baseURL: 'https://localhost:7116/api',  // HTTPS + 7116
  headers: {
    'Content-Type': 'application/json'
  }
})

export default api