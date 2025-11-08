-- ========================================
-- Create Roles Table
-- ========================================
CREATE TABLE Roles (
    role_id INT IDENTITY(1,1) PRIMARY KEY,
    role_name VARCHAR(50) NOT NULL,
    description VARCHAR(255),
    created_by INT,
    is_deleted BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME
);


INSERT INTO Roles(role_name, description) VALUES
('Administrator', 'System administrator'),
('User', 'Normal user');

-- ========================================
-- Create Users Table
-- ========================================
CREATE TABLE Users (
    user_id INT IDENTITY(1,1) PRIMARY KEY,
    role_id INT NOT NULL,
    user_name VARCHAR(100) NOT NULL,
    image VARCHAR(200),
    phone_number VARCHAR(50),
    email VARCHAR(150),
    password VARCHAR(255) NOT NULL,
    created_by INT,
    is_active BIT DEFAULT 1,
    login_at DATETIME,
    is_deleted BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME,
    FOREIGN KEY (role_id) REFERENCES Roles(role_id)
);

INSERT INTO Users(role_id, user_name, email, password, created_by)
VALUES (
    1,
    'admin',
    'admin@example.com',
    'admin123',
    1
);

-- ========================================
-- Menus
-- ========================================
CREATE TABLE Menus (
    menu_id INT IDENTITY(1,1) PRIMARY KEY,
    menu_name VARCHAR(150) NOT NULL,
    status BIT DEFAULT 1,
    created_by INT,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME
);

INSERT INTO Menus (menu_name, status, created_by)
VALUES
('Dashboard', 1, 1),
('Products', 1, 1),
('Sizes', 1, 1),
('Categories', 1, 1),
('Brand', 1, 1),
('Inventory', 1, 1),
('Suppliers', 1, 1),
('Orders', 1, 1),
('Setting', 1, 1),
('Accounts', 1, 1),
('Permission', 1, 1),
('Role', 1, 1);

-- ========================================
-- Permission (User ←→ Menu Many to Many)
-- ========================================
CREATE TABLE Permission (
    menu_id INT NOT NULL,
    user_id INT NOT NULL,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME,
    PRIMARY KEY(menu_id, user_id),
    FOREIGN KEY(menu_id) REFERENCES Menus(menu_id),
    FOREIGN KEY(user_id) REFERENCES Users(user_id)
);


INSERT INTO Permission (menu_id, user_id)
SELECT menu_id, 1 FROM Menus;

-- ========================================
-- Categories
-- ========================================
CREATE TABLE Categories (
    category_id INT IDENTITY(1,1) PRIMARY KEY,
    category_name VARCHAR(150),
    created_by INT,
    is_deleted BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME
);

-- ========================================
-- Sizes
-- ========================================
CREATE TABLE Sizes (
    size_id INT IDENTITY(1,1) PRIMARY KEY,
    size_name VARCHAR(100),
    created_by INT,
    is_deleted BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME
);

-- ========================================
-- Brands
-- ========================================
CREATE TABLE Brands (
    brand_id INT IDENTITY(1,1) PRIMARY KEY,
    brand_name VARCHAR(150),
    created_by INT,
    is_deleted BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME
);

-- ========================================
-- Items
-- ========================================
CREATE TABLE Items (
    item_id INT IDENTITY(1,1) PRIMARY KEY,
    category_id INT,
    brand_id INT,
    size_id INT,
    item_name VARCHAR(150),
    item_code VARCHAR(150),
    unit_price DECIMAL(18,2),
    cost_price DECIMAL(18,2),
    discount DECIMAL(18,2),
    tax DECIMAL(18,2),
    description VARCHAR(255),
    created_by INT,
    is_deleted BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME,
    FOREIGN KEY(category_id) REFERENCES Categories(category_id),
    FOREIGN KEY(brand_id) REFERENCES Brands(brand_id),
    FOREIGN KEY(size_id) REFERENCES Sizes(size_id)
);

-- ========================================
-- Suppliers
-- ========================================
CREATE TABLE Suppliers (
    supplier_id INT IDENTITY(1,1) PRIMARY KEY,
    supplier_name VARCHAR(150),
    phone_number VARCHAR(50),
    email VARCHAR(150),
    address VARCHAR(255),
    created_by INT,
    is_deleted BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME
);

-- ========================================
-- Purchases
-- ========================================
CREATE TABLE Purchases (
    purchase_id INT IDENTITY(1,1) PRIMARY KEY,
    supplier_id INT,
    price DECIMAL(18,2),
    delivery_fee DECIMAL(18,2),
    tax DECIMAL(18,2),
    payment DECIMAL(18,2),
    balance DECIMAL(18,2),
    description VARCHAR(255),
    created_by INT,
    is_deleted BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME,
    FOREIGN KEY(supplier_id) REFERENCES Suppliers(supplier_id)
);

-- Purchase Details
CREATE TABLE Purchase_details (
    purchase_id INT,
    item_id INT,
    cost_price DECIMAL(18,2),
    quantity INT,
    is_deleted BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME,
    PRIMARY KEY(purchase_id, item_id),
    FOREIGN KEY(purchase_id) REFERENCES Purchases(purchase_id),
    FOREIGN KEY(item_id) REFERENCES Items(item_id)
);

-- ========================================
-- Stock_masters
-- ========================================
CREATE TABLE Stock_masters (
    stock_id INT IDENTITY(1,1) PRIMARY KEY,
    description VARCHAR(255),
    created_by INT,
    is_deleted BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME
);

-- Stock_details
CREATE TABLE Stock_details (
    stock_id INT,
    item_id INT,
    quantity INT,
    expire_date DATE,
    is_deleted BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME,
    PRIMARY KEY(stock_id, item_id),
    FOREIGN KEY(stock_id) REFERENCES Stock_masters(stock_id),
    FOREIGN KEY(item_id) REFERENCES Items(item_id)
);

-- ========================================
-- Orders
-- ========================================
CREATE TABLE Orders (
    order_id INT IDENTITY(1,1) PRIMARY KEY,
    subtotal_price DECIMAL(18,2),
    discount DECIMAL(18,2),
    tax DECIMAL(18,2),
    payment_method VARCHAR(50),
    total_price DECIMAL(18,2),
    created_by INT,
    is_deleted BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME
);

-- Order_details
CREATE TABLE Order_details (
    order_id INT,
    item_id INT,
    quantity INT,
    unit_price DECIMAL(18,2),
    total_price DECIMAL(18,2),
    is_deleted BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME,
    PRIMARY KEY(order_id, item_id),
    FOREIGN KEY(order_id) REFERENCES Orders(order_id),
    FOREIGN KEY(item_id) REFERENCES Items(item_id)
);
