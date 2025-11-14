<!-- src/views/Departamentos.vue -->
<template>
  <div class="container">
    <h2>Departamentos</h2>

    <form @submit.prevent="crearDepartamento" class="form">
      <input v-model="nuevo.nombreDepartamento" placeholder="Nombre del departamento" required />
      <button type="submit">Crear</button>
    </form>

    <div v-if="error" class="error">{{ error }}</div>

    <table class="table">
      <thead>
        <tr>
          <th>ID</th>
          <th>Nombre</th>
          <th>Acciones</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="d in departamentos" :key="d.idDepartamento">
          <td>{{ d.idDepartamento }}</td>
          <td>
            <input v-if="editando === d.idDepartamento" v-model="d.nombreDepartamento" />
            <span v-else>{{ d.nombreDepartamento }}</span>
          </td>
          <td>
            <button v-if="editando === d.idDepartamento" @click="actualizar(d)">Guardar</button>
            <button v-else @click="editando = d.idDepartamento">Editar</button>
            <button @click="eliminar(d.idDepartamento)" class="delete">Eliminar</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '@/services/api'

const departamentos = ref([])
const nuevo = ref({ nombreDepartamento: '' })
const editando = ref(null)
const error = ref('')

onMounted(async () => {
  await cargar()
})

const cargar = async () => {
  try {
    const res = await api.get('/departamentos')
    departamentos.value = res.data
    error.value = ''
  } catch (e) {
    error.value = 'Error al cargar departamentos'
  }
}

const crearDepartamento = async () => {
  try {
    await api.post('/departamentos', nuevo.value)
    nuevo.value.nombreDepartamento = ''
    await cargar()
  } catch (e) {
    error.value = e.response?.data?.message || 'Error al crear'
  }
}

const actualizar = async (d) => {
  try {
    await api.put(`/departamentos/${d.idDepartamento}`, { nombreDepartamento: d.nombreDepartamento })
    editando.value = null
    await cargar()
  } catch (e) {
    error.value = e.response?.data?.message || 'Error al actualizar'
  }
}

const eliminar = async (id) => {
  if (!confirm('¿Eliminar?')) return
  try {
    await api.delete(`/departamentos/${id}`)
    await cargar()
  } catch (e) {
    error.value = e.response?.data?.message || 'No se puede eliminar (hay ciudades asociadas)'
  }
}
</script>

<style scoped>
.container { padding: 2rem; max-width: 900px; margin: 0 auto; }
h2 { color: #2c3e50; text-align: center; }
.form { display: flex; gap: 1rem; margin-bottom: 1.5rem; }
input, button { padding: 0.5rem; font-size: 1rem; }
button { background: #27ae60; color: white; border: none; cursor: pointer; }
button:hover { background: #219a52; }
.delete { background: #c0392b; margin-left: 0.5rem; }
.delete:hover { background: #a93226; }
.table { width: 100%; border-collapse: collapse; margin-top: 1rem; }
th, td { border: 1px solid #ddd; padding: 0.75rem; text-align: left; }
th { background: #f4f4f4; }
.error { color: red; margin: 1rem 0; font-weight: bold; }
</style>