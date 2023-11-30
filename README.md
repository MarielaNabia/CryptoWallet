# CryptoWallet
Sistema de Compra y Venta de Criptomonedas

Este documento proporciona una descripción general de un proyecto de desarrollo de una aplicación para un sistema de compra y venta de criptomonedas. El proyecto se enfoca en implementar un sistema de gestión de cuentas que permite a los usuarios realizar transacciones, consultar saldos y realizar operaciones con diferentes tipos de cuentas, como pesos ($), dólares (USD) y criptomonedas (por ejemplo, BTC).
Descripción del Proyecto

El proyecto tiene como objetivo desarrollar una aplicación que permita a los usuarios gestionar sus cuentas y realizar las siguientes acciones:

    Consultar el saldo de la cuenta.
    Realizar depósitos y retiros de dinero en las cuentas fiduciarias (pesos y dólares).
    Transferir fondos entre cuentas del mismo cliente.
    Comprar criptomonedas.
    Consultar los últimos movimientos de una cuenta.

Las diferencias principales entre los tipos de cuenta son las siguientes:

    Las cuentas fiduciarias (pesos y dólares) poseen CBU, alias y número de cuenta.
    Las cuentas de criptomonedas solo poseen una dirección UUID (Universally Unique Identifier) para dirigir las acciones de depósito y transferencia.
    Para comprar criptomonedas, los fondos deben partir de la cuenta en pesos, convertirse a dólares y luego a criptomonedas.

Además, la aplicación debe consume APIs de criptomonedas y de divisas para obtener cotizaciones en tiempo real.
Arquitectura del Proyecto

El proyecto sigue una arquitectura que consta de varias capas, cada una con su función específica:

    Capa ServicioAPICotizaciones: Responsable de obtener las cotizaciones en tiempo real.

    Capa Controller: Esta capa se encarga de recibir las solicitudes HTTP entrantes, procesarlas y enviar las respuestas correspondientes. Gestiona las rutas y la interacción con los endpoints de la API.

    Capa DataAccess: Responsable de gestionar la interacción con la base de datos. Proporciona una interfaz eficiente para acceder y manipular datos almacenados en la base de datos.

    Capa Entities: Define y modela las estructuras de datos que representan las entidades de la aplicación. Estas entidades corresponden a las tablas de la base de datos y representan objetos del mundo real con sus propiedades y relaciones.

    Capa Repositories: Contiene las clases correspondientes para realizar el repositorio genérico y la unidad de trabajo.

    Capa Core: Es el componente central que proporciona funcionalidades compartidas, estructura y lógica común esenciales para el funcionamiento de la aplicación. Incluye subniveles como Helper (lógica útil para el proyecto), Interfaces (definición de interfaces), Models (definición de modelos de datos), y Services (lógica de servicio).

Gestión de Ramas (Git)

En este proyecto, se utiliza una estrategia de gestión de ramas que facilita la colaboración y el seguimiento de cada implementación. Algunas de las prácticas clave son las siguientes:

    Todas las ramas se crean a partir de la rama develop, que representa la última versión de desarrollo y donde se integran las características antes de su liberación.

    Cada Pull Request (PR) está asociado a un número de historia o característica específica, lo que facilita la trazabilidad de los cambios en relación con los requisitos del proyecto.

    Se sigue una política clara de gestión de ramas, donde master es la rama principal de producción y develop es la rama de desarrollo.

Este enfoque de gestión de ramas asegura un flujo de trabajo ordenado y la capacidad de rastrear el progreso y los cambios en cada característica o mejora. Facilita la colaboración efectiva entre los miembros del equipo y mantiene un control de versiones preciso.

Para obtener más detalles sobre la implementación y las tecnologías utilizadas, consulte el proyecto modelo llamado "CryptoWallet" en la carpeta correspondiente.

Nota: Este README es un modelo general. Asegúrese de personalizarlo de acuerdo a su proyecto y las tecnologías específicas utilizadas.