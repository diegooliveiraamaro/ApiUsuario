-- Cria��o da tabela exemplo
CREATE TABLE Usuarios (
    UsuarioID SERIAL PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    DataCriacao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Cria��o de um �ndice na coluna Email para melhorar o desempenho das buscas
CREATE INDEX idx_email ON Usuarios (Email);

-- Cria��o de uma trigger para preencher automaticamente o campo DataCriacao
-- caso o valor n�o seja fornecido
CREATE OR REPLACE FUNCTION trg_set_data_criacao()
RETURNS TRIGGER AS $$
BEGIN
    -- Verifica se a DataCriacao � nula e atribui a data atual
    IF NEW.DataCriacao IS NULL THEN
        NEW.DataCriacao := CURRENT_TIMESTAMP;
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Vincula��o da trigger � tabela Usuarios
CREATE TRIGGER before_insert_usuario
BEFORE INSERT ON Usuarios
FOR EACH ROW
EXECUTE FUNCTION trg_set_data_criacao();

-- Cria��o de uma procedure para limpar registros antigos
CREATE OR REPLACE PROCEDURE LimparRegistrosAntigos()
LANGUAGE plpgsql AS $$
BEGIN
    -- Exclui registros da tabela Usuarios que tenham mais de 1 ano
    DELETE FROM Usuarios WHERE DataCriacao < CURRENT_DATE - INTERVAL '1 year';
END;
$$;

-- Exemplo de chamada para a procedure (essa chamada pode ser feita em algum processo agendado)
-- CALL LimparRegistrosAntigos();
