<template>
  <div class="home">
    <h1>Sistema CRUD Oracle</h1>
    <p class="subtitle">Gestión completa de datos con auditoría</p>

    <div class="cards">
      <!-- DEPARTAMENTOS -->
      <router-link to="/departamentos" class="card">
        <div class="icon">Map</div>
        <h3>Departamentos</h3>
        <p>Gestión territorial</p>
      </router-link>

      <!-- CIUDADES -->
      <router-link to="/ciudades" class="card">
        <div class="icon">City</div>
        <h3>Ciudades</h3>
        <p>Relacionadas con departamentos</p>
      </router-link>

      <!-- USUARIOS -->
      <router-link to="/usuarios" class="card">
        <div class="icon">Users</div>
        <h3>Usuarios</h3>
        <p>Crear, editar, eliminar</p>
      </router-link>

      <!-- ACCIONES -->
      <router-link to="/acciones" class="card">
        <div class="icon">List</div>
        <h3>Acciones</h3>
        <p>Tipos de operaciones</p>
      </router-link>

      <!-- LOGS -->
      <router-link to="/logs" class="card">
        <div class="icon">History</div>
        <h3>Logs</h3>
        <p>Auditoría completa</p>
      </router-link>
    </div>
  </div>
</template>

<style scoped>
.home {
  padding: 2rem;
  text-align: center;
  min-height: 80vh;
  display: flex;
  flex-direction: column;
  justify-content: center;
  background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
}

h1 {
  color: #2c3e50;
  margin-bottom: 0.5rem;
  font-size: 2.5rem;
  font-weight: 700;
}

.subtitle {
  color: #7f8c8d;
  margin-bottom: 3rem;
  font-size: 1.2rem;
  font-weight: 400;
}

.cards {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 1.8rem;
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1rem;
}

.card {
  background: white;
  padding: 1.8rem;
  border-radius: 16px;
  box-shadow: 0 6px 16px rgba(0, 0, 0, 0.1);
  text-decoration: none;
  color: #2c3e50;
  transition: all 0.3s ease;
  text-align: center;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.card:hover {
  transform: translateY(-10px);
  box-shadow: 0 16px 32px rgba(0, 0, 0, 0.15);
}

.icon {
  font-size: 2.8rem;
  margin-bottom: 0.8rem;
  color: #3498db;
}

.card h3 {
  margin: 0.5rem 0;
  color: #2c3e50;
  font-size: 1.3rem;
  font-weight: 600;
}

.card p {
  color: #95a5a6;
  font-size: 0.95rem;
  margin: 0;
  line-height: 1.4;
}
</style>