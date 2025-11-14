<template>
  <nav class="navbar">
    <div class="nav-brand">CRUD Oracle</div>
    <div class="nav-links">
      <router-link to="/">Inicio</router-link>
      <router-link to="/departamentos">Departamentos</router-link>
      <router-link to="/ciudades">Ciudades</router-link>
      <router-link to="/usuarios">Usuarios</router-link>
      <router-link to="/acciones">Acciones</router-link>
      <router-link to="/logs">Logs</router-link>
    </div>
  </nav>
</template>

<style scoped>
.navbar {
  background: #2c3e50;
  color: white;
  padding: 1rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-weight: bold;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.nav-brand {
  font-size: 1.5rem;
  font-weight: 700;
  letter-spacing: 0.5px;
}

.nav-links {
  display: flex;
  gap: 1.5rem;
}

.nav-links a {
  color: white;
  text-decoration: none;
  font-size: 1.1rem;
  padding: 0.5rem 0.75rem;
  border-radius: 6px;
  transition: all 0.2s ease;
  position: relative;
}

.nav-links a:hover {
  background: rgba(255, 255, 255, 0.15);
  transform: translateY(-1px);
}

.nav-links a.router-link-active {
  background: #3498db;
  font-weight: 600;
}
</style>