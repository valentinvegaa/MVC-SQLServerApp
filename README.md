# MVC-SQLServerApp
Tarea para Kunder

Prueba:
Primera parte

    Levantar un ambiente local de asp mvc 4 con una base de datos sql server express 2008
    Levantar un sitio web "hola mundo"

Segunda parte

    Armar un esquema de bd, que cuando la aplicación parta, detecte que si no está presente, lo despliegue (SQL create table ...., etc)
    1.1. El esquema consiste de una sola tabla "requests", que tiene como campos un id (correlativo) y un segundo campo "request", que es de tipo texto
    Crear un servicio GET (/getRequest?id=XX) que devuelva el id pedido.
    Crear un servicio PUT (/putRequest) que inserte en la BD un nuevo request. En el campo request, deberá insertar el contenido del PUT.
    Crear un servicio POST (/count) que entregue el conteo de requests en la base de datos

Consideraciones

    El código debe quedar en un repositorio github para que pueda ser probado por nosotros
    El repositorio debe tener ignorados los archivos que no deben ser versionados
    Para las conexiones a la base de datos, poner el string de conexion en el archivo config de la aplicación
    Para usar la base de datos, no usar conexiones ni sentencias sql de manera directa. Se debe usar el ORM de ASP MVC, con objetos para las tablas
