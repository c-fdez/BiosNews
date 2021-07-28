--Carlos Fernandez / 6284165-4 /
--Fernando Juanico / 5160986-1 /

USE Master
GO

IF EXISTS (select * from sys.databases where name = 'BiosNews')
begin
	DROP DATABASE BiosNews
end
GO

CREATE DATABASE BiosNews ON
(
	NAME=BiosNews,
	FILENAME='C:\Users\Ferna_9ddcx0y\OneDrive\Escritorio\BIOS\Segundo\DISEÑO 1\BiosNews.mdf'
)
GO

USE BiosNews

CREATE TABLE Periodistas
(
	CI INT primary key NOT NULL CHECK (LEN(CI)=8),
	Nombre varchar(50) NOT NULL,
	Mail varchar (20) NOT NULL CHECK(Mail LIKE ('^[a-zA-Z0-9.!#$%&*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\\.[a-zA-Z0-9-]+)*$')),
	/*(Mail LIKE '%_@__%.__%' AND PATINDEX('%[^a-z,0-9,@,.,_,\-]%', Mail) = 0), */
	Activo bit NOT NULL default (1)
)
CREATE TABLE Empleados
(
	NomUsu char(10) PRIMARY KEY NOT NULL CHECK (LEN(NomUsu)=10),
	Pass char(7) NOT NULL CHECK (/*LEN(Pass)=7 AND*/ Pass LIKE '[a-z]{4}[0-9]{3}')
)
CREATE TABLE Secciones
(
	CodInt char(5) Primary Key NOT NULL CHECK(/*LEN(CodInt)=5 AND*/ CodInt LIKE '[a-z]{5}'),
	Nombre varchar(20) NOT NULL,
	Activo bit NOT NULL default(1) 
)
CREATE TABLE Noticias
(
	CodInt varchar(20) Primary Key NOT NULL,
	FechaPub Date NOT NULL,
		CHECK (FechaPub >= GETDATE()),
	Titulo varchar(50) NOT NULL,
	Contenido varchar(1000) NOT NULL,
	Importancia INT NOT NULL default (5),
		CHECK (Importancia>0 AND Importancia<6),
	NomUsu char(10) FOREIGN KEY References Empleados(NomUsu) NOT NULL	 
)
CREATE TABLE Nacionales
(
	CodInt varchar(20) FOREIGN KEY References Noticias(CodInt) NOT NULL PRIMARY KEY,
	CodSec char(5) FOREIGN KEY References Secciones(CodInt) NOT NULL
)
CREATE TABLE Internacionales
(
	CodInt varchar(20) FOREIGN KEY References Noticias(CodInt) NOT NULL PRIMARY KEY,
	Pais varchar(20) NOT NULL
)
CREATE TABLE Escriben
(
	CodInt varchar(20) FOREIGN KEY References Noticias(CodInt) NOT NULL,
	CI INT FOREIGN KEY References Periodistas(CI) NOT NULL,
	Primary Key (CodInt, CI)
)

go	
CREATE PROC AltaPeriodista
--ALTER PROC AltaPeriodista
@CI INT,
@Nombre varchar(50),
@Mail varchar (20)

AS
begin
if exists (select * from Periodistas where CI = @CI AND Activo = 1) 
	return -1 --Ya existe el periodista
if exists (select * from Periodistas where CI = @CI AND Activo = 0)
	BEGIN 
		UPDATE Periodistas SET Activo = 1, Nombre = @Nombre, Mail = @Mail WHERE CI = @CI 
		return 1
	END 
	Else
	BEGIN
		INSERT Periodistas (CI,Nombre,Mail) VALUES (@CI,@Nombre,@Mail)
		RETURN 1;
	END
END
go

CREATE PROCEDURE ModificarPeriodista @CI INT,@Nombre varchar(50),@Mail varchar (20) AS
Begin
--Revisar nuevamente porque en clase se hizo de otra forma
	if Not(EXISTS(Select * From Periodistas Where CI = @CI AND Activo = 1))
		return -1 --El periodista no existe
	UPDATE Periodistas
	SET Nombre = @Nombre, Mail = @Mail
	WHERE CI = @CI
	
	IF(@@Error=0)
		RETURN 1
	else
		RETURN 0
End
go

CREATE PROCEDURE BajaPeriodista @CI INT AS
Begin
	if Not(EXISTS(Select * From Periodistas Where CI = @CI))
		return -1
	if EXISTS(Select * From Escriben Where CI = @CI)
		BEGIN
			UPDATE Periodistas 
			SET Activo = 0
			WHERE CI = @CI
			return 1
		END 
	
	DELETE FROM Periodistas WHERE CI = @CI
	IF(@@ERROR=0)
		RETURN 1
	ELSE
		RETURN 0
END
go

CREATE PROCEDURE BuscarPeriodista @CI INT AS
BEGIN 
	SELECT * FROM Periodistas WHERE CI = @CI;
END
go

CREATE PROCEDURE BuscarPeriodistaActivo @CI INT AS
BEGIN 
	SELECT * FROM Periodistas WHERE CI = @CI AND Activo = 1;
END
go

CREATE PROCEDURE AltaEmpleado
--ALTER PROC AltaEmpleado
@NomUsu char(10),
@Pass char(7)
AS
begin
if exists (select * from Empleados where NomUsu = @NomUsu)
	return -1 --Ya existe el empleado

INSERT Empleados VALUES (@NomUsu, @Pass)
IF(@@Error=0)
		RETURN 1;
	ELSE
		RETURN 0;
end
GO

CREATE PROCEDURE BuscarEmpleado @NomUsu char(10) AS
BEGIN
	Select * from Empleados where NomUsu = @NomUsu
END
go

CREATE PROCEDURE Logueo @NomUsu char(10), @Pass char(7) AS
Begin
	Select *
	From Empleados 
	Where NomUsu = @NomUsu AND Pass = @Pass
End
go	

CREATE PROCEDURE AltaSeccion @CodInt char(5), @Nombre varchar(20) AS
BEGIN
if exists (select * from Secciones where CodInt = @CodInt AND Activo = 1) 
	return -1 --Ya existe la seccion
if exists (select * from Secciones where CodInt = @CodInt AND Activo = 0)
	BEGIN
		UPDATE Secciones
		SET Activo = 1, Nombre = @Nombre
		WHERE CodInt = @CodInt
		Return 1
	END

INSERT Secciones (CodInt,Nombre)Values (@CodInt, @Nombre)
		RETURN 1;
END
go

CREATE PROCEDURE ModificarSeccion @CodInt char(5), @Nombre varchar(20) AS
Begin
	if Not(EXISTS(Select * From Secciones Where CodInt = @CodInt AND Activo = 1))
		return -1 --La seccion no existe
	
	UPDATE Secciones
	SET Nombre = @Nombre
	WHERE CodInt = @CodInt
	
	IF(@@Error=0)
		RETURN 1
	else
		RETURN 0
End
go

CREATE PROCEDURE BajaSeccion @CodInt char(5) AS
Begin
	if Not(EXISTS(Select * From Secciones Where CodInt = @CodInt))
		return -1
	if exists (select * from Nacionales where CodSec = @CodInt)
		BEGIN
			UPDATE Secciones
			SET Activo = 0
			WHERE CodInt = @CodInt
			return 1
		END
	
	DELETE FROM Secciones WHERE CodInt = @CodInt
	IF(@@ERROR=0)
		RETURN 1
	ELSE
		RETURN 0
END
go

CREATE PROCEDURE BuscarSeccionActivo @CodInt char(5) AS
BEGIN
	select * from Secciones WHERE CodInt = @CodInt AND Activo = 1
END
go

CREATE PROCEDURE BuscarSeccion @CodInt char(5) AS
BEGIN
	select * from Secciones WHERE CodInt = @CodInt 
END
go

CREATE PROCEDURE ListarSecciones AS
BEGIN
	Select * from Secciones WHERE Activo = 1
END
go

CREATE PROCEDURE ListarPeriodistas AS
BEGIN
	Select * from Periodistas WHERE Activo = 1
END
go

CREATE PROCEDURE AltaNacionales @CodInt varchar(20),@FechaPub Date,@Titulo varchar(50),@Contenido varchar(1000),
	@Importancia INT,@NomUsu char(10),@CodSec char(5) AS
BEGIN

if not exists (select * from Secciones where CodInt = @CodSec AND Activo = 1)
	return -1 --No existe la Seccion
if exists (select * from Noticias where CodInt = @CodInt)
	return -2 --Ya existe la noticia
if not exists (select * from Empleados where NomUsu = @NomUsu)
	return -3

declare @err int
set @err = 0

begin tran
	INSERT Noticias VALUES (@CodInt ,@FechaPub,@Titulo,@Contenido,@Importancia,@NomUsu)
	set @err = @err + @@ERROR
	INSERT Nacionales VALUES (@CodInt,@CodSec)
	set @err = @err + @@ERROR
if @err = 0
begin
	COMMIT TRAN
	return 1
end
else
begin
	ROLLBACK TRAN
	return -4
end
END
GO

CREATE PROCEDURE AltaInternacionales @CodInt varchar(20),@FechaPub Date,@Titulo varchar(50),@Contenido varchar(1000),
	@Importancia INT,@NomUsu char(10),@Pais varchar(20) AS
BEGIN

if exists (select * from Noticias where CodInt = @CodInt)
	return -1 --Ya existe la noticia
if not exists (select * from Empleados where NomUsu = @NomUsu)
	return -2

declare @err int
set @err = 0

begin tran
	INSERT Noticias VALUES (@CodInt ,@FechaPub,@Titulo,@Contenido,@Importancia,@NomUsu)
	set @err = @err + @@ERROR
	INSERT Internacinales VALUES (@CodInt,@Pais)
	set @err = @err + @@ERROR
if @err = 0
begin
	COMMIT TRAN
	return 1
end
else
begin
	ROLLBACK TRAN
	return -4
end
END
GO

CREATE PROCEDURE AltaEscriben @CodInt varchar(20), @CI Int AS
BEGIN
	if NOT EXISTS(Select * from Noticias Where @CodInt = CodInt)
		return -1
	if NOT EXISTS(Select * from Periodistas Where @CI = CI AND Activo = 1)
		return -2
	if EXISTS(Select * from Escriben Where CI = @CI AND @CodInt = CodInt)
		return -3

INSERT Escriben VALUES (@CodInt, @CI)
IF(@@Error=0)
		RETURN 1;
	ELSE
		RETURN 0;	
END
GO

CREATE PROCEDURE ModificarNacionales @CodInt varchar(20),@FechaPub Date,@Titulo varchar(50),@Contenido varchar(1000),
	@Importancia INT,@NomUsu char(10),@CodSec char(5) AS
BEGIN

if not exists (select * from Secciones where CodInt = @CodSec AND Activo = 1)
	return -1 --No existe la Seccion
if not exists (select * from Noticias where CodInt = @CodInt)
	return -2 --No existe la noticia
if not exists (select * from Empleados where NomUsu = @NomUsu)
	return -3

declare @err int
set @err = 0

begin tran
	Update Noticias 
	Set @FechaPub = FechaPub,@Titulo = Titulo, @Contenido = Contenido, @Importancia = Importancia, @NomUsu = NomUsu
	Where @CodInt = CodInt
	set @err = @err + @@ERROR
	
	UPDATE Nacionales 
	SET @CodSec = CodSec
	Where @CodInt = CodInt 
	set @err = @err + @@ERROR
	
if @err = 0
begin
	COMMIT TRAN
	return 1
end
else
begin
	ROLLBACK TRAN
	return -4
end
END
GO

CREATE PROCEDURE ModificarInternacionales @CodInt varchar(20),@FechaPub Date,@Titulo varchar(50),@Contenido varchar(1000),
	@Importancia INT,@NomUsu char(10),@Pais varchar(20) AS
BEGIN

if not exists (select * from Noticias where CodInt = @CodInt)
	return -1 --No existe la noticia
if not exists (select * from Empleados where NomUsu = @NomUsu)
	return -2

declare @err int
set @err = 0

begin tran
	Update Noticias 
	Set @FechaPub = FechaPub,@Titulo = Titulo, @Contenido = Contenido, @Importancia = Importancia, @NomUsu = NomUsu
	Where @CodInt = CodInt
	set @err = @err + @@ERROR
	
	UPDATE Internacionales 
	SET @Pais = Pais
	Where @CodInt = CodInt 
	set @err = @err + @@ERROR
	
if @err = 0
begin
	COMMIT TRAN
	return 1
end
else
begin
	ROLLBACK TRAN
	return -4
end
END
GO

CREATE PROCEDURE BajaEscriben @CodInt varchar(20) AS
BEGIN
	if NOT EXISTS(Select * from Escriben where CodInt = @CodInt)
		return -1
	Delete From Escriben Where CodInt = @CodInt
	return 1
END
GO

CREATE PROCEDURE ListarEscriben @CodInt varchar(20) AS
BEGIN
	Select * from Escriben Where CodInt = @CodInt
END
GO

CREATE PROCEDURE BuscarNacional @CodInt varchar(20) AS
BEGIN
	Select b.*,a.CodSec 
	from Nacionales as a
	INNER JOIN Noticias as b ON(a.CodInt = b.CodInt)
	where a.CodInt = @CodInt
END
GO

CREATE PROCEDURE BuscarInternacional @CodInt varchar(20) AS
BEGIN
	Select b.*,a.Pais 
	from Internacionales as a
	INNER JOIN Noticias as b ON(a.CodInt = b.CodInt)
	where a.CodInt = @CodInt
END
GO

CREATE PROCEDURE ListarDefault AS
BEGIN
	Select * from Noticias
	Where FechaPub <= GETDATE() AND FechaPub >= DATEADD(DAY,-5, GETDATE()) 
END
GO

CREATE PROCEDURE ListarPublicados AS
BEGIN
	Select * from Noticias
	Where FechaPub <= GETDATE()
END
GO