CREATE DATABASE ShoppingCart;

USE ShoppingCart;

CREATE TABLE ShoppingCart(
    ID INT PRIMARY KEY AUTO_INCREMENT
);

CREATE TABLE ShoppingCartItems(
    ID INT PRIMARY KEY AUTO_INCREMENT,
	ShoppingCartId INT NOT NULL,
    ProductId INT NOT NULL,
	Name NVARCHAR(100) NOT NULL,
	Description NVARCHAR(500) NULL,
	Amount DECIMAL NOT NULL,
	Currency NVARCHAR(5) NOT NULL
);

ALTER TABLE ShoppingCartItems ADD CONSTRAINT FOREIGN KEY(ShoppingCartId)
REFERENCES ShoppingCart (ID);

CREATE INDEX ShoppingCartItems_ShoppingCartId 
ON ShoppingCartItems (ShoppingCartId);

