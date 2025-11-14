<template>
  <div class="container">
    <h2>Logs</h2>

    <div v-if="loading">Cargando...</div>
    
    <div v-else-if="error" class="error">{{ error }}</div>
    
    <table v-else class="table">
      <thead>
        <tr>
          <th>ID Log</th>
          <th>CC</th>
          <th>Fecha Ingreso</th>
          <th>Tipo de Acción</th>
        </tr>
      </thead>
      <tbody>
        <tr v-if="logs.length === 0">
          <td colspan="4">No hay logs registrados</td>
        </tr>
        <tr v-for="log in logs" :key="log.idLog">
          <td>{{ log.idLog }}</td>
          <td>{{ log.cc }}</td>
          <td>{{ formatearFecha(log.fechaIngreso) }}</td>
          <td>{{ log.tipoAccion }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '@/services/api'

const logs = ref([])
const loading = ref(false)
const error = ref('')

onMounted(async () => {
  await cargar()
})

const cargar = async () => {
  loading.value = true
  error.value = ''
  
  try {
    const res = await api.get('/logs')
    logs.value = res.data
  } catch (e) {
    error.value = 'Error al cargar los logs'
  } finally {
    loading.value = false
  }
}

const formatearFecha = (fecha) => {
  return new Date(fecha).toLocaleString('es-CO')
}
</script>

<style scoped>
.container { padding: 2rem; max-width: 1200px; margin: 0 auto; }
h2 { color: #2c3e50; text-align: center; }
.table { width: 100%; border-collapse: collapse; margin-top: 1rem; }
th, td { border: 1px solid #ddd; padding: 0.75rem; text-align: left; }
th { background: #f4f4f4; }
.error { color: red; margin: 1rem 0; font-weight: bold; }
</style>