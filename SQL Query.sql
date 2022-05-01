USE master
GO
--Criar Banco de dados
CREATE DATABASE BDGT
GO
--Selecionar Banco de Dados
USE BDGT
--Criar Tabela Usuario que será usada para salvar dados de login
CREATE TABLE tblUsuario(
Usuario_Cod int NOT NULL PRIMARY KEY IDENTITY,
Usuario_Nome varchar(200) NOT NULL,
Usuario_Senha varchar(200) NOT NULL
)

--Criar Tabela Fornecedor
CREATE TABLE tblFornecedor(
Fornecedor_Cod int NOT NULL PRIMARY KEY IDENTITY,
Fornecedor_Nome varchar(200) NOT NULL UNIQUE,
Fornecedor_End varchar(200),
Fornecedor_Cel varchar(11)
)

--Criar Tabela de Produtos, com referência a tabela fornecedor
CREATE TABLE tblProduto(
Produto_Cod int NOT NULL PRIMARY KEY IDENTITY,
Produto_Nome varchar(200) NOT NULL UNIQUE,
Produto_Marca varchar(200),
Produto_Preco decimal(19,2),
Cod_Fornecedor int NOT NULL,
CONSTRAINT FK_Cod_Fornecedor FOREIGN KEY ([Cod_Fornecedor]) REFERENCES [tblFornecedor] (Fornecedor_Cod)
ON DELETE CASCADE ON UPDATE CASCADE
)

--Inserir Dados de login padrão na tabela
Insert into tblUsuario 
(Usuario_Nome, Usuario_Senha)
values
('ADMIN', 'ADMIN')
