# Despliegue del API en Render (Docker)

Repositorio: `trycore-evm-api`

## Requisitos previos

- Cuenta en [Render](https://render.com)
- Repositorio en GitHub: `https://github.com/jupapema11/trycore-evm-api`
- Rama `main` con el `Dockerfile` en la raíz del repo

## Paso 1: Crear base de datos PostgreSQL

1. En Render → **New +** → **PostgreSQL**
2. Nombre: `trycore-evm-db`
3. Plan: **Free** (o el que prefieras)
4. Crear y copiar la **Internal Database URL** (formato `postgresql://...`)

## Paso 2: Crear Web Service del API

1. **New +** → **Web Service**
2. Conectar el repo `trycore-evm-api`
3. Configuración:
   - **Name:** `trycore-evm-api`
   - **Region:** la más cercana a tus usuarios
   - **Branch:** `main`
   - **Runtime:** `Docker`
   - **Dockerfile Path:** `./Dockerfile`
   - **Instance type:** Free

## Paso 3: Variables de entorno del API

En **Environment** del servicio web:

| Variable | Valor |
|----------|--------|
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `DATABASE_URL` | Pegar la **Internal Database URL** de PostgreSQL |
| `CORS_ORIGINS` | URL del frontend en Render (ej. `https://trycore-evm-frontend.onrender.com`) |

> Si despliegas el frontend después, vuelve y actualiza `CORS_ORIGINS` con la URL real.

## Paso 4: Desplegar

1. Click **Create Web Service**
2. Espera el build (5–10 min la primera vez)
3. Verifica:
   - Health: `https://TU-API.onrender.com/health`
   - Swagger: `https://TU-API.onrender.com/`

Las migraciones EF se aplican automáticamente al iniciar el contenedor.

## Paso 5: Probar el API

```bash
curl https://TU-API.onrender.com/health
curl https://TU-API.onrender.com/api/Projects
```

## Notas

- Render inyecta `PORT`; el contenedor escucha en ese puerto.
- No subas contraseñas al repo; usa solo variables de entorno en Render.
- En plan Free el servicio se “duerme” tras inactividad; la primera petición puede tardar ~1 min.
