<template>
  <div class="container">
    <h2>Acciones</h2>

    <div v-if="loading">Cargando...</div>
    
    <div v-else-if="error" class="error">{{ error }}</div>
    
    <table v-else class="table">
      <thead>
        <tr>
          <th>Tipo de Acción</th>
        </tr>
      </thead>
      <tbody>
        <tr v-if="acciones.length === 0">
          <td>No hay acciones registradas</td>
        </tr>
        <tr v-for="accion in acciones" :key="accion.tipoAccion">
          <td>{{ accion.tipoAccion }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '@/services/api'

const acciones = ref([])
const loading = ref(false)
const error = ref('')

onMounted(async () => {
  await cargar()
})

const cargar = async () => {
  loading.value = true
  error.value = ''
  
  try {
    const res = await api.get('/acciones')
    acciones.value = res.data
  } catch (e) {
    error.value = 'Error al cargar las acciones'
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.container { padding: 2rem; max-width: 900px; margin: 0 auto; }
h2 { color: #2c3e50; text-align: center; }
.table { width: 100%; border-collapse: collapse; margin-top: 1rem; }
th, td { border: 1px solid #ddd; padding: 0.75rem; text-align: left; }
th { background: #f4f4f4; }
.error { color: red; margin: 1rem 0; font-weight: bold; }
</style>