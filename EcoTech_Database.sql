/* EcoTech - estructura y datos mínimos de demostración - SQL Server */
IF DB_ID(N'EcoTech') IS NULL CREATE DATABASE EcoTech;
GO
USE EcoTech;
GO

CREATE TABLE dbo.roles (id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,nombre VARCHAR(50) NOT NULL,descripcion VARCHAR(255) NULL,activo BIT NOT NULL CONSTRAINT DF_roles_activo DEFAULT 1,CONSTRAINT UQ_roles_nombre UNIQUE(nombre));
GO
CREATE TABLE dbo.usuarios (id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,usuario VARCHAR(50) NOT NULL,password_hash VARCHAR(255) NOT NULL,rol_id INT NOT NULL,activo BIT NOT NULL CONSTRAINT DF_usuarios_activo DEFAULT 1,fecha_creacion DATETIME2 NOT NULL CONSTRAINT DF_usuarios_fecha_creacion DEFAULT sysdatetime(),CONSTRAINT UQ_usuarios_usuario UNIQUE(usuario),CONSTRAINT FK_usuarios_roles FOREIGN KEY(rol_id) REFERENCES dbo.roles(id));
GO
CREATE TABLE dbo.categorias (id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,nombre VARCHAR(100) NOT NULL,descripcion VARCHAR(255) NULL,activo BIT NOT NULL CONSTRAINT DF_categorias_activo DEFAULT 1,CONSTRAINT UQ_categorias_nombre UNIQUE(nombre));
GO
CREATE TABLE dbo.clientes (id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,nombre VARCHAR(150) NOT NULL,nit VARCHAR(30) NULL,email VARCHAR(100) NULL,telefono VARCHAR(30) NULL,direccion VARCHAR(255) NULL,limite_credito DECIMAL(18,2) NOT NULL CONSTRAINT DF_clientes_limite_credito DEFAULT 0,activo BIT NOT NULL CONSTRAINT DF_clientes_activo DEFAULT 1,fecha_registro DATETIME2 NOT NULL CONSTRAINT DF_clientes_fecha_registro DEFAULT sysdatetime(),CONSTRAINT UQ_clientes_nit UNIQUE(nit),CONSTRAINT CK_clientes_limite_credito CHECK(limite_credito>=0));
GO
CREATE TABLE dbo.proveedores (id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,nombre VARCHAR(150) NOT NULL,nit VARCHAR(30) NULL,email VARCHAR(100) NULL,telefono VARCHAR(30) NULL,direccion VARCHAR(255) NULL,plazo_pago INT NOT NULL CONSTRAINT DF_proveedores_plazo_pago DEFAULT 0,activo BIT NOT NULL CONSTRAINT DF_proveedores_activo DEFAULT 1,fecha_registro DATETIME2 NOT NULL CONSTRAINT DF_proveedores_fecha_registro DEFAULT sysdatetime(),CONSTRAINT UQ_proveedores_nit UNIQUE(nit),CONSTRAINT CK_proveedores_plazo_pago CHECK(plazo_pago>=0));
GO
CREATE TABLE dbo.empresa (id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,nombre VARCHAR(150) NOT NULL,nit VARCHAR(30) NULL,direccion VARCHAR(255) NULL,telefono VARCHAR(30) NULL,email VARCHAR(100) NULL,activo BIT NOT NULL CONSTRAINT DF_empresa_activo DEFAULT 1,CONSTRAINT UQ_empresa_nit UNIQUE(nit));
GO
CREATE TABLE dbo.cuentas_contables (id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,codigo VARCHAR(20) NOT NULL,nombre VARCHAR(150) NOT NULL,tipo VARCHAR(30) NOT NULL,descripcion VARCHAR(255) NULL,activo BIT NOT NULL CONSTRAINT DF_cuentas_contables_activo DEFAULT 1,CONSTRAINT UQ_cuentas_contables_codigo UNIQUE(codigo));
GO
CREATE TABLE dbo.permisos (id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,rol_id INT NOT NULL,recurso VARCHAR(100) NOT NULL,accion VARCHAR(50) NOT NULL,CONSTRAINT UQ_permisos_rol_recurso_accion UNIQUE(rol_id,recurso,accion),CONSTRAINT FK_permisos_roles FOREIGN KEY(rol_id) REFERENCES dbo.roles(id));
GO
CREATE TABLE dbo.productos (id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,codigo VARCHAR(50) NOT NULL,nombre VARCHAR(150) NOT NULL,categoria_id INT NOT NULL,precio_costo DECIMAL(18,2) NOT NULL,precio_venta DECIMAL(18,2) NOT NULL,stock DECIMAL(18,2) NOT NULL CONSTRAINT DF_productos_stock DEFAULT 0,stock_minimo DECIMAL(18,2) NOT NULL CONSTRAINT DF_productos_stock_minimo DEFAULT 0,activo BIT NOT NULL CONSTRAINT DF_productos_activo DEFAULT 1,fecha_registro DATETIME2 NOT NULL CONSTRAINT DF_productos_fecha_registro DEFAULT sysdatetime(),CONSTRAINT UQ_productos_codigo UNIQUE(codigo),CONSTRAINT FK_productos_categorias FOREIGN KEY(categoria_id) REFERENCES dbo.categorias(id));
GO
CREATE TABLE dbo.ordenes_compra (id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,proveedor_id INT NOT NULL,usuario_id INT NOT NULL,fecha DATETIME2 NOT NULL,fecha_entrega DATETIME2 NULL,subtotal DECIMAL(18,2) NOT NULL,impuesto DECIMAL(18,2) NOT NULL,total DECIMAL(18,2) NOT NULL,estado VARCHAR(50) NOT NULL,CONSTRAINT FK_ordenes_compra_proveedor FOREIGN KEY(proveedor_id) REFERENCES dbo.proveedores(id),CONSTRAINT FK_ordenes_compra_usuario FOREIGN KEY(usuario_id) REFERENCES dbo.usuarios(id));
GO
CREATE TABLE dbo.detalles_orden (id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,orden_id INT NOT NULL,producto_id INT NOT NULL,cantidad DECIMAL(18,2) NOT NULL,precio DECIMAL(18,2) NOT NULL,subtotal DECIMAL(18,2) NOT NULL,CONSTRAINT CK_detalles_orden_cantidad CHECK(cantidad>0),CONSTRAINT CK_detalles_orden_precio CHECK(precio>=0),CONSTRAINT CK_detalles_orden_subtotal CHECK(subtotal>=0),CONSTRAINT FK_detalles_orden_orden FOREIGN KEY(orden_id) REFERENCES dbo.ordenes_compra(id),CONSTRAINT FK_detalles_orden_producto FOREIGN KEY(producto_id) REFERENCES dbo.productos(id));
GO
CREATE TABLE dbo.recepciones_compra (id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,orden_id INT NOT NULL,usuario_id INT NOT NULL,fecha DATETIME2 NOT NULL,observacion VARCHAR(500) NULL,CONSTRAINT FK_recepciones_compra_orden FOREIGN KEY(orden_id) REFERENCES dbo.ordenes_compra(id),CONSTRAINT FK_recepciones_compra_usuario FOREIGN KEY(usuario_id) REFERENCES dbo.usuarios(id));
GO
CREATE TABLE dbo.movimientos_inventario (id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,producto_id INT NOT NULL,usuario_id INT NOT NULL,tipo VARCHAR(20) NOT NULL,cantidad DECIMAL(18,2) NOT NULL,stock_anterior DECIMAL(18,2) NOT NULL,stock_nuevo DECIMAL(18,2) NOT NULL,fecha DATETIME2 NOT NULL,concepto VARCHAR(150) NOT NULL,referencia VARCHAR(100) NULL,CONSTRAINT FK_movimientos_inventario_producto FOREIGN KEY(producto_id) REFERENCES dbo.productos(id),CONSTRAINT FK_movimientos_inventario_usuario FOREIGN KEY(usuario_id) REFERENCES dbo.usuarios(id));
GO
CREATE TABLE dbo.asientos_contables (id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,fecha DATETIME2 NOT NULL CONSTRAINT DF_asientos_contables_fecha DEFAULT sysdatetime(),concepto VARCHAR(255) NOT NULL,cuenta_id INT NOT NULL,debe DECIMAL(18,2) NOT NULL CONSTRAINT DF_asientos_contables_debe DEFAULT 0,haber DECIMAL(18,2) NOT NULL CONSTRAINT DF_asientos_contables_haber DEFAULT 0,usuario_id INT NOT NULL,CONSTRAINT CK_asientos_debe CHECK(debe>=0),CONSTRAINT CK_asientos_haber CHECK(haber>=0),CONSTRAINT CK_asientos_movimiento CHECK((debe>0 AND haber=0) OR (haber>0 AND debe=0)),CONSTRAINT FK_asientos_cuenta FOREIGN KEY(cuenta_id) REFERENCES dbo.cuentas_contables(id),CONSTRAINT FK_asientos_usuario FOREIGN KEY(usuario_id) REFERENCES dbo.usuarios(id));
GO
CREATE TABLE dbo.caja (id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,usuario_id INT NOT NULL,fecha DATETIME2 NOT NULL CONSTRAINT DF_caja_fecha DEFAULT sysdatetime(),concepto VARCHAR(255) NOT NULL,ingreso DECIMAL(18,2) NOT NULL CONSTRAINT DF_caja_ingreso DEFAULT 0,egreso DECIMAL(18,2) NOT NULL CONSTRAINT DF_caja_egreso DEFAULT 0,saldo DECIMAL(18,2) NOT NULL CONSTRAINT DF_caja_saldo DEFAULT 0,tipo VARCHAR(20) NOT NULL,CONSTRAINT CK_caja_ingreso CHECK(ingreso>=0),CONSTRAINT CK_caja_egreso CHECK(egreso>=0),CONSTRAINT CK_caja_tipo CHECK(tipo='EGRESO' OR tipo='INGRESO'),CONSTRAINT CK_caja_movimiento CHECK((tipo='INGRESO' AND ingreso>0 AND egreso=0) OR (tipo='EGRESO' AND egreso>0 AND ingreso=0)),CONSTRAINT FK_caja_usuarios FOREIGN KEY(usuario_id) REFERENCES dbo.usuarios(id));
GO

INSERT INTO dbo.roles(nombre,descripcion,activo) VALUES
('Administrador','Acceso completo al sistema',1),('Vendedor','Gestión de ventas y clientes',1),('Contador','Gestión contable y financiera',1);
GO
INSERT INTO dbo.usuarios(usuario,password_hash,rol_id,activo,fecha_creacion) VALUES
('demo_ecotech','100000.WUVePN/s17gnaGHUQIoN7g==.ZWi1V1Kq16OOAAgRe1nKg+RiwXo+nOmH0aZac+yun5c=',1,1,sysdatetime());
GO
INSERT INTO dbo.categorias(nombre,descripcion,activo) VALUES
('Electrónica','Productos electrónicos y tecnológicos',1),('Oficina','Artículos y suministros de oficina',1),('Hogar','Productos para el hogar',1),('Limpieza','Productos de limpieza y mantenimiento',1),('Accesorios','Accesorios y complementos tecnológicos',1),('Tecnología','Productos tecnológicos y accesorios',0);
GO
INSERT INTO dbo.proveedores(nombre,nit,email,telefono,direccion,plazo_pago,activo,fecha_registro) VALUES
('Tech Solutions Dominicana','101234567','ventas@techsolutions.com','809-555-1001','Santo Domingo, RD',30,1,sysdatetime()),
('Distribuidora Office Plus','101234568','ventas@officeplus.com','809-555-1002','Santo Domingo, RD',15,1,sysdatetime()),
('Importadora Global Tech','101234569','contacto@globaltech.com','809-555-1003','Santiago, RD',45,1,sysdatetime()),
('Electro RD','101234570','ventas@electrord.com','809-555-1004','Santo Domingo, RD',30,1,sysdatetime()),
('Caribe Supplies','101234571','info@caribesupplies.com','809-555-1005','La Romana, RD',20,0,sysdatetime());
GO
PRINT 'EcoTech creada correctamente.';
PRINT 'Usuario demo: demo_ecotech';
PRINT 'Contraseña demo: EcoTech123*';
GO
