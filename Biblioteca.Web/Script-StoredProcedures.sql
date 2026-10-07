-- ================================================================
-- Script de Stored Procedures para BibliotecaDB
-- Requiere: BibliotecaDB ya existente con tablas Libros, Autores,
--           Socios, Prestamos y DetallePrestamo.
-- ================================================================
USE BibliotecaDB;
GO

-- ================================================================
-- STORED PROCEDURES: Libros
-- ================================================================

-- Listado de libros activos con nombre del autor
CREATE OR ALTER PROCEDURE usp_Libros_Listar
AS
BEGIN
    SELECT l.LibroId, l.Titulo, l.ISBN, l.Ejemplares, l.Activo,
           l.AutorId,
           a.Nombre AS NombreAutor
    FROM Libros l
    INNER JOIN Autores a ON a.AutorId = l.AutorId
    WHERE l.Activo = 1
    ORDER BY l.Titulo;
END
GO

-- Búsqueda de libros por título (LIKE)
CREATE OR ALTER PROCEDURE usp_Libros_BuscarPorTitulo
    @Titulo NVARCHAR(200)
AS
BEGIN
    SELECT l.LibroId, l.Titulo, l.ISBN, l.Ejemplares, l.Activo,
           l.AutorId,
           a.Nombre AS NombreAutor
    FROM Libros l
    INNER JOIN Autores a ON a.AutorId = l.AutorId
    WHERE l.Activo = 1
      AND l.Titulo LIKE N'%' + @Titulo + N'%'
    ORDER BY l.Titulo;
END
GO

-- Obtener libro por Id
CREATE OR ALTER PROCEDURE usp_Libros_ObtenerPorId
    @LibroId INT
AS
BEGIN
    SELECT l.LibroId, l.Titulo, l.ISBN, l.Ejemplares, l.Activo,
           l.AutorId,
           a.Nombre AS NombreAutor
    FROM Libros l
    INNER JOIN Autores a ON a.AutorId = l.AutorId
    WHERE l.LibroId = @LibroId AND l.Activo = 1;
END
GO

-- Insertar libro
CREATE OR ALTER PROCEDURE usp_Libros_Insertar
    @Titulo     NVARCHAR(200),
    @ISBN       NVARCHAR(20),
    @AutorId    INT,
    @Ejemplares INT
AS
BEGIN
    INSERT INTO Libros (Titulo, ISBN, AutorId, Ejemplares)
    VALUES (@Titulo, @ISBN, @AutorId, @Ejemplares);
END
GO

-- Actualizar libro
CREATE OR ALTER PROCEDURE usp_Libros_Actualizar
    @LibroId    INT,
    @Titulo     NVARCHAR(200),
    @ISBN       NVARCHAR(20),
    @AutorId    INT,
    @Ejemplares INT
AS
BEGIN
    UPDATE Libros
    SET Titulo     = @Titulo,
        ISBN       = @ISBN,
        AutorId    = @AutorId,
        Ejemplares = @Ejemplares
    WHERE LibroId = @LibroId;
END
GO

-- Eliminación lógica de libro (pone Activo = 0)
CREATE OR ALTER PROCEDURE usp_Libros_Eliminar
    @LibroId INT
AS
BEGIN
    UPDATE Libros SET Activo = 0 WHERE LibroId = @LibroId;
END
GO

-- ================================================================
-- STORED PROCEDURES: Socios
-- ================================================================

-- Listado de socios activos
CREATE OR ALTER PROCEDURE usp_Socios_Listar
AS
BEGIN
    SELECT SocioId, DNI, Nombre, Email, Activo
    FROM Socios
    WHERE Activo = 1
    ORDER BY Nombre;
END
GO

-- Insertar socio
CREATE OR ALTER PROCEDURE usp_Socios_Insertar
    @DNI    NVARCHAR(15),
    @Nombre NVARCHAR(100),
    @Email  NVARCHAR(100)
AS
BEGIN
    INSERT INTO Socios (DNI, Nombre, Email)
    VALUES (@DNI, @Nombre, @Email);
END
GO

-- Verifica si el DNI ya existe (excluyendo al socio actual; al crear enviar @SocioId = 0)
CREATE OR ALTER PROCEDURE usp_Socios_ExisteDNI
    @DNI     NVARCHAR(15),
    @SocioId INT
AS
BEGIN
    SELECT COUNT(*)
    FROM Socios
    WHERE DNI = @DNI AND SocioId <> @SocioId AND Activo = 1;
END
GO

-- ================================================================
-- STORED PROCEDURES: Autores
-- ================================================================

-- Listado de autores activos (para lista desplegable)
CREATE OR ALTER PROCEDURE usp_Autores_Listar
AS
BEGIN
    SELECT AutorId, Nombre, Nacionalidad, Activo
    FROM Autores
    WHERE Activo = 1
    ORDER BY Nombre;
END
GO

-- ================================================================
-- STORED PROCEDURES: Reporte de préstamos
-- ================================================================

-- Reporte de préstamos por intervalo de fechas
CREATE OR ALTER PROCEDURE usp_Prestamos_Reporte
    @Desde DATE,
    @Hasta DATE
AS
BEGIN
    SELECT
        p.PrestamoId,
        p.FechaPrestamo,
        p.FechaLimite,
        p.Estado,
        s.Nombre   AS NombreSocio,
        s.DNI,
        l.Titulo   AS TituloLibro,
        a.Nombre   AS NombreAutor,
        dp.FechaDevolucion,
        dp.Multa
    FROM Prestamos p
    INNER JOIN Socios           s  ON s.SocioId  = p.SocioId
    INNER JOIN DetallePrestamo  dp ON dp.PrestamoId = p.PrestamoId
    INNER JOIN Libros           l  ON l.LibroId   = dp.LibroId
    INNER JOIN Autores          a  ON a.AutorId   = l.AutorId
    WHERE p.FechaPrestamo >= @Desde
      AND p.FechaPrestamo <= @Hasta
    ORDER BY p.FechaPrestamo DESC, p.PrestamoId, l.Titulo;
END
GO

-- Insertar nuevo préstamo y su detalle
CREATE OR ALTER PROCEDURE usp_Prestamos_Insertar
    @SocioId       INT,
    @LibroId       INT,
    @FechaPrestamo DATE,
    @FechaLimite   DATE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @NuevoId INT;

    INSERT INTO Prestamos (SocioId, FechaPrestamo, FechaLimite, Estado)
    VALUES (@SocioId, @FechaPrestamo, @FechaLimite, 'Pendiente');

    SET @NuevoId = SCOPE_IDENTITY();

    INSERT INTO DetallePrestamo (PrestamoId, LibroId, FechaDevolucion, Multa)
    VALUES (@NuevoId, @LibroId, NULL, NULL);

    SELECT @NuevoId;
END
GO

