CREATE TABLE IF NOT EXISTS consolidated_balance (
    id SERIAL PRIMARY KEY,
    total_credit DECIMAL(18, 2) DEFAULT 0.00,
    total_debit DECIMAL(18, 2) DEFAULT 0.00,
    balance DECIMAL(18, 2) DEFAULT 0.00,
    last_updated TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);