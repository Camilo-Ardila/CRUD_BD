<!-- src/views/Ciudades.vue -->
<template>
  <div class="container">
    <h2>Ciudades</h2>

    <form @submit.prevent="crearCiudad" class="form">
      <input v-model="nuevo.nombreCiudad" placeholder="Nombre de la ciudad" required />
      <select v-model="nuevo.idDepartamento" required>
        <option value="">Selecciona departamento</option>
        <option v-for="d in departamentos" :key="d.idDepartamento" :value="d.idDepartamento">
          {{ d.nombreDepartamento }}
        </option>
      </select>
      <button type="submit">Crear</button>
    </form>

    <div v-if="error" class="error">{{ error }}</div>

    <table class="table">
      <thead>
        <tr>
          <th>ID</th>
          <th>Ciudad</th>
          <th>Departamento</th>
          <th>Acciones</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="c in ciudades" :key="c.idCiudad">
          <td>{{ c.idCiudad }}</td>
          <td>
            <input v-if="editando === c.idCiudad" v-model="c.nombreCiudad" />
            <span v-else>{{ c.nombreCiudad }}</span>
          </td>
          <td>
            <select v-if="editando === c.idCiudad" v-model="c.idDepartamento">
              <option v-for="d in departamentos" :key="d.idDepartamento" :value="d.idDepartamento">
                {{ d.nombreDepartamento }}
              </option>
            </select>
            <span v-else>{{ c.departamento }}</span>
          </td>
          <td>
            <button v-if="editando === c.idCiudad" @click="actualizar(c)" class="save">Guardar</button>
            <button v-else @click="editando = c.idCiudad" class="edit">Editar</button>
            <button @click="eliminar(c.idCiudad)" class="delete">Eliminar</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '@/services/api'

const ciudades = ref([])
const departamentos = ref([])
const nuevo = ref({ nombreCiudad: '', idDepartamento: '' })
const editando = ref(null)
const error = ref('')

onMounted(async () => {
  await cargarTodo()
})

const cargarTodo = async () => {
  try {
    const [resC, resD] = await Promise.all([
      api.get('/ciudades'),
      api.get('/departamentos')
    ])
    ciudades.value = resC.data
    departamentos.value = resD.data
    error.value = ''
  } catch (e) {
    error.value = 'Error al cargar datos'
  }
}

const crearCiudad = async () => {
  try {
    await api.post('/ciudades', nuevo.value)
    nuevo.value = { nombreCiudad: '', idDepartamento: '' }
    await cargarTodo()
  } catch (e) {
    error.value = e.response?.data?.message || 'Error al crear'
  }
}

const actualizar = async (c) => {
  try {
    await api.put(`/ciudades/${c.idCiudad}`, {
      idCiudad: c.idCiudad,  // ← ESTO FALTABA
      nombreCiudad: c.nombreCiudad,
      idDepartamento: c.idDepartamento
    })
    editando.value = null
    await cargarTodo()
  } catch (e) {
    error.value = e.response?.data?.message || 'Error al actualizar'
  }
}

const eliminar = async (id) => {
  if (!confirm('¿Eliminar?')) return
  try {
    await api.delete(`/ciudades/${id}`)
    await cargarTodo()
  } catch (e) {
    error.value = e.response?.data?.message || 'No se puede eliminar'
  }
}
</script>

<style scoped>
.container { padding: 2rem; max-width: 1000px; margin: 0 auto; }
h2 { color: #2c3e50; text-align: center; }
.form { display: flex; gap: 1rem; margin-bottom: 1.5rem; flex-wrap: wrap; }
input, select, button { padding: 0.5rem; font-size: 1rem; }
button { color: white; border: none; cursor: pointer; }
button { background: #27ae60; }
button:hover { background: #219a52; }
.edit { background: #3498db; }
.edit:hover { background: #2980b9; }
.save { background: #27ae60; }
.save:hover { background: #219a52; }
.delete { background: #c0392b; margin-left: 0.5rem; }
.delete:hover { background: #a93226; }
.table { width: 100%; border-collapse: collapse; margin-top: 1rem; }
th, td { border: 1px solid #ddd; padding: 0.75rem; text-align: left; }
th { background: #f4f4f4; }
.error { color: red; margin: 1rem 0; font-weight: bold; }
</style>