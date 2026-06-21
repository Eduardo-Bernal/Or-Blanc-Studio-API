CREATE DATABASE OrBlancDB;
GO

USE OrBlancDB
GO


CREATE TABLE Cliente (
    id_cliente  UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    nome        VARCHAR(100)     NOT NULL,
    telefone    VARCHAR(20)      NOT NULL,
    email       VARCHAR(100)     NULL,
    senha       VARBINARY(32)    NOT NULL,

    CONSTRAINT PK_Cliente       PRIMARY KEY (id_cliente),
    CONSTRAINT UQ_Cliente_Email UNIQUE (email),
    CONSTRAINT UQ_Cliente_Fone  UNIQUE (telefone)
);
GO

CREATE TABLE Profissional (
    id_profissional UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    nome            VARCHAR(100)     NOT NULL,
    especialidade   VARCHAR(100)     NOT NULL,
    telefone        VARCHAR(20)      NOT NULL,
    email           VARCHAR(100)     NULL,
    ativo           BIT              NOT NULL DEFAULT 1,
    senha           VARBINARY(32)    NOT NULL,
    imagem          VARBINARY(MAX)     NULL,    

    CONSTRAINT PK_Profissional PRIMARY KEY (id_profissional)
);
GO

CREATE TABLE Servico (
    id_servico  INT            IDENTITY(1,1) NOT NULL,
    nome        VARCHAR(100)   NOT NULL,
    descricao   VARCHAR(255)   NULL,
    valor       DECIMAL(10, 2) NOT NULL,
    ativo       BIT            NOT NULL DEFAULT 1,
    imagem      VARBINARY(MAX)   NULL,
    CONSTRAINT PK_Servico       PRIMARY KEY (id_servico),
    CONSTRAINT CK_Servico_Valor CHECK (valor >= 0)
);
GO

CREATE TABLE Agendamento (
    id_agendamento   INT              IDENTITY(1,1) NOT NULL,
    id_cliente       UNIQUEIDENTIFIER NOT NULL,
    id_servico       INT              NOT NULL,
    data_hora_inicio DATETIME         NOT NULL,
    data_hora_fim    DATETIME         NOT NULL,
    status           VARCHAR(20)      NOT NULL DEFAULT 'Agendado',
    observacao       VARCHAR(500)     NULL,

    CONSTRAINT PK_Agendamento PRIMARY KEY (id_agendamento),

    CONSTRAINT FK_Agendamento_Cliente
        FOREIGN KEY (id_cliente) REFERENCES Cliente(id_cliente)
        ON DELETE CASCADE,

    CONSTRAINT FK_Agendamento_Servico
        FOREIGN KEY (id_servico) REFERENCES Servico(id_servico)
        ON DELETE NO ACTION,

    CONSTRAINT CK_Agendamento_Horario
        CHECK (data_hora_fim > data_hora_inicio),

    CONSTRAINT CK_Agendamento_Status
        CHECK (status IN ('Agendado', 'Confirmado', 'Concluído', 'Cancelado'))
);
GO

CREATE TABLE Agendamento_Profissional (
    id_agendamento  INT              NOT NULL,
    id_profissional UNIQUEIDENTIFIER NOT NULL,

    CONSTRAINT PK_Agendamento_Profissional
        PRIMARY KEY (id_agendamento, id_profissional),        

    CONSTRAINT FK_AgPro_Agendamento
        FOREIGN KEY (id_agendamento) REFERENCES Agendamento(id_agendamento)
        ON DELETE CASCADE,                                    

    CONSTRAINT FK_AgPro_Profissional
        FOREIGN KEY (id_profissional) REFERENCES Profissional(id_profissional)
        ON DELETE NO ACTION                                   
);
GO


CREATE INDEX IX_Agendamento_Cliente
    ON Agendamento (id_cliente);
GO

CREATE INDEX IX_Agendamento_DataHora
    ON Agendamento (data_hora_inicio);
GO

CREATE INDEX IX_AgendamentoProfissional_Profissional
    ON Agendamento_Profissional (id_profissional);
GO


DECLARE @cli1 UNIQUEIDENTIFIER = NEWID();
DECLARE @cli2 UNIQUEIDENTIFIER = NEWID();
DECLARE @cli3 UNIQUEIDENTIFIER = NEWID();
DECLARE @cli4 UNIQUEIDENTIFIER = NEWID();
DECLARE @cli5 UNIQUEIDENTIFIER = NEWID();

INSERT INTO Cliente (id_cliente, nome, telefone, email, senha) VALUES
    (@cli1, 'Ana Paula Silva',  '11987654321', 'ana.paula@email.com',      HASHBYTES('SHA2_256', 'ana123')),
    (@cli2, 'Beatriz Souza',    '11976543210', 'beatriz.souza@email.com',  HASHBYTES('SHA2_256', 'bea123')),
    (@cli3, 'Carlos Oliveira',  '11965432109', 'carlos.oliveira@email.com',HASHBYTES('SHA2_256', 'car123')),
    (@cli4, 'Diana Lima',       '11954321098', 'diana.lima@email.com',     HASHBYTES('SHA2_256', 'dia123')),
    (@cli5, 'Eduardo Santos',   '11943210987', 'eduardo.santos@email.com', HASHBYTES('SHA2_256', 'edu123'));

DECLARE @pro1 UNIQUEIDENTIFIER = NEWID();
DECLARE @pro2 UNIQUEIDENTIFIER = NEWID();
DECLARE @pro3 UNIQUEIDENTIFIER = NEWID();
DECLARE @pro4 UNIQUEIDENTIFIER = NEWID();
DECLARE @pro5 UNIQUEIDENTIFIER = NEWID();

INSERT INTO Profissional (id_profissional, nome, especialidade, telefone, ativo, senha, imagem) VALUES
    (@pro1, 'Fernanda Costa',   'Cabelereira', '11911112222', 1, HASHBYTES('SHA2_256', 'fer123'), NULL),
    (@pro2, 'Gabriela Mendes',  'Manicure',    '11922223333', 1, HASHBYTES('SHA2_256', 'gab123'), NULL),
    (@pro3, 'Helena Rodrigues', 'Esteticista', '11933334444', 1, HASHBYTES('SHA2_256', 'hel123'), NULL),
    (@pro4, 'Igor Ferreira',    'Cabelereiro', '11944445555', 1, HASHBYTES('SHA2_256', 'igo123'), NULL),
    (@pro5, 'Juliana Martins',  'Sobrancelha', '11955556666', 0, HASHBYTES('SHA2_256', 'jul123'), NULL);

INSERT INTO Servico (nome, descricao, valor, ativo, imagem) VALUES
    ('Corte Feminino',     'Corte de cabelo feminino',            60.00, 1, NULL),
    ('Corte Masculino',    'Corte de cabelo masculino',           40.00, 1, NULL),
    ('Coloração',          'Tintura completa do cabelo',         150.00, 1, NULL),
    ('Escova',             'Escova modeladora',                   50.00, 1, NULL),
    ('Manicure',           'Unhas das mãos com esmalte comum',    30.00, 1, NULL),
    ('Pedicure',           'Unhas dos pés com esmalte comum',     35.00, 1, NULL),
    ('Design Sobrancelha', 'Design e depilação de sobrancelha',   25.00, 1, NULL),
    ('Limpeza de Pele',    'Limpeza profunda e hidratação',      120.00, 1, NULL);



                    
 DECLARE @age1 INT, @age2 INT, @age3 INT,

        @age4 INT, @age5 INT, @age6 INT,
        @age7 INT, @age8 INT;

INSERT INTO Agendamento (id_cliente, id_servico, data_hora_inicio, data_hora_fim, status, observacao) VALUES
    (@cli1, 1, '2026-06-10 09:00', '2026-06-10 10:00', 'Confirmado', 'Cliente prefere franja lateral');
SET @age1 = SCOPE_IDENTITY();

INSERT INTO Agendamento (id_cliente, id_servico, data_hora_inicio, data_hora_fim, status, observacao) VALUES
    (@cli2, 5, '2026-06-10 09:00', '2026-06-10 09:45', 'Agendado', NULL);
SET @age2 = SCOPE_IDENTITY();

INSERT INTO Agendamento (id_cliente, id_servico, data_hora_inicio, data_hora_fim, status, observacao) VALUES
    (@cli3, 4, '2026-06-10 10:30', '2026-06-10 11:30', 'Agendado', NULL);
SET @age3 = SCOPE_IDENTITY();

INSERT INTO Agendamento (id_cliente, id_servico, data_hora_inicio, data_hora_fim, status, observacao) VALUES
    (@cli4, 8, '2026-06-10 11:00', '2026-06-10 12:00', 'Agendado', 'Pele sensível');
SET @age4 = SCOPE_IDENTITY();

INSERT INTO Agendamento (id_cliente, id_servico, data_hora_inicio, data_hora_fim, status, observacao) VALUES
    (@cli5, 2, '2026-06-10 14:00', '2026-06-10 14:45', 'Agendado', NULL);
SET @age5 = SCOPE_IDENTITY();

INSERT INTO Agendamento (id_cliente, id_servico, data_hora_inicio, data_hora_fim, status, observacao) VALUES
    (@cli1, 6, '2026-06-11 10:00', '2026-06-11 10:50', 'Agendado', NULL);
SET @age6 = SCOPE_IDENTITY();

INSERT INTO Agendamento (id_cliente, id_servico, data_hora_inicio, data_hora_fim, status, observacao) VALUES
    (@cli2, 3, '2026-06-11 09:00', '2026-06-11 11:00', 'Confirmado', 'Cor desejada: castanho escuro');
SET @age7 = SCOPE_IDENTITY();

INSERT INTO Agendamento (id_cliente, id_servico, data_hora_inicio, data_hora_fim, status, observacao) VALUES
    (@cli3, 2, '2026-06-11 15:00', '2026-06-11 15:45', 'Cancelado', 'Cliente desmarcou');
SET @age8 = SCOPE_IDENTITY();

-- ✅ Vínculos na tabela intermediária
INSERT INTO Agendamento_Profissional (id_agendamento, id_profissional) VALUES
    (@age1, @pro1),
    (@age2, @pro2),
    (@age3, @pro1),
    (@age4, @pro3),
    (@age5, @pro4),
    (@age6, @pro2),
    (@age7, @pro1),
    (@age8, @pro4);
GO


-- ✅ View atualizada com JOIN na tabela intermediária
CREATE VIEW VW_AgendaCompleta AS
SELECT
    a.id_agendamento,
    c.id_cliente,
    c.nome                                                AS nome_cliente,
    c.telefone                                            AS telefone_cliente,
    p.id_profissional,
    p.nome                                                AS nome_profissional,
    p.especialidade,
    s.nome                                                AS nome_servico,
    s.valor                                               AS valor_servico,
    a.data_hora_inicio,
    a.data_hora_fim,
    DATEDIFF(MINUTE, a.data_hora_inicio, a.data_hora_fim) AS duracao_minutos,
    a.status,
    a.observacao
FROM
    Agendamento             a
    INNER JOIN Cliente      c   ON c.id_cliente      = a.id_cliente
    INNER JOIN Servico      s   ON s.id_servico      = a.id_servico
    INNER JOIN Agendamento_Profissional ap ON ap.id_agendamento  = a.id_agendamento   -- ✅ via intermediária
    INNER JOIN Profissional p   ON p.id_profissional = ap.id_profissional;
GO


CREATE TRIGGER TRG_VerificaConflito
ON Agendamento_Profissional                              -- ✅ trigger agora na tabela intermediária
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @id_agendamento  INT;
    DECLARE @id_profissional UNIQUEIDENTIFIER;
    DECLARE @data_inicio     DATETIME;
    DECLARE @data_fim        DATETIME;
    DECLARE @conflitos       INT;

    SELECT
        @id_agendamento  = i.id_agendamento,
        @id_profissional = i.id_profissional,
        @data_inicio     = a.data_hora_inicio,
        @data_fim        = a.data_hora_fim
    FROM inserted i
    INNER JOIN Agendamento a ON a.id_agendamento = i.id_agendamento;

    SELECT @conflitos = COUNT(*)
    FROM Agendamento_Profissional ap
    INNER JOIN Agendamento a ON a.id_agendamento = ap.id_agendamento
    WHERE
        ap.id_profissional = @id_profissional
        AND ap.id_agendamento <> @id_agendamento
        AND a.status NOT IN ('Cancelado')
        AND @data_inicio < a.data_hora_fim
        AND @data_fim    > a.data_hora_inicio;

    IF @conflitos > 0
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR('CONFLITO DE HORÁRIO: o profissional já tem agendamento nesse período.', 16, 1);
    END
END;
GO


CREATE PROCEDURE SP_RealizarAgendamento
    @id_cliente       UNIQUEIDENTIFIER,
    @id_profissional  UNIQUEIDENTIFIER,
    @id_servico       INT,
    @data_hora_inicio DATETIME,
    @data_hora_fim    DATETIME,
    @observacao       VARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Cliente WHERE id_cliente = @id_cliente)
    BEGIN PRINT 'ERRO: Cliente não encontrado.'; RETURN; END

    IF NOT EXISTS (SELECT 1 FROM Profissional WHERE id_profissional = @id_profissional AND ativo = 1)
    BEGIN PRINT 'ERRO: Profissional não encontrado ou está inativo.'; RETURN; END

    IF NOT EXISTS (SELECT 1 FROM Servico WHERE id_servico = @id_servico AND ativo = 1)
    BEGIN PRINT 'ERRO: Serviço não encontrado ou está inativo.'; RETURN; END

    IF @data_hora_fim <= @data_hora_inicio
    BEGIN PRINT 'ERRO: O horário de término deve ser maior que o horário de início.'; RETURN; END

    DECLARE @novo_id INT;

    INSERT INTO Agendamento (id_cliente, id_servico, data_hora_inicio, data_hora_fim, status, observacao)
    VALUES (@id_cliente, @id_servico, @data_hora_inicio, @data_hora_fim, 'Agendado', @observacao);

    SET @novo_id = SCOPE_IDENTITY();

    -- ✅ Vincula o profissional na tabela intermediária
    INSERT INTO Agendamento_Profissional (id_agendamento, id_profissional)
    VALUES (@novo_id, @id_profissional);

    PRINT 'Agendamento realizado com sucesso! ID: ' + CAST(@novo_id AS VARCHAR);
END;
GO


SELECT * FROM VW_AgendaCompleta ORDER BY data_hora_inicio;
GO

SELECT * FROM VW_AgendaCompleta WHERE nome_profissional = 'Fernanda Costa' ORDER BY data_hora_inicio;
GO

SELECT * FROM VW_AgendaCompleta WHERE CAST(data_hora_inicio AS DATE) = '2026-06-10' ORDER BY data_hora_inicio;
GO

SELECT * FROM VW_AgendaCompleta WHERE status = 'Confirmado' ORDER BY data_hora_inicio;
GO

SELECT id_cliente, nome, telefone, email FROM Cliente ORDER BY nome;
GO

SELECT id_profissional, nome, especialidade, telefone FROM Profissional WHERE ativo = 1 ORDER BY nome;
GO

SELECT id_servico, nome, descricao, valor FROM Servico WHERE ativo = 1 ORDER BY valor;
GO

UPDATE Agendamento SET status = 'Cancelado'  WHERE id_agendamento = 5;
GO

UPDATE Agendamento SET status = 'Confirmado' WHERE id_agendamento = 2;
GO


UPDATE Servico 
set imagem = 0x48656C6C6F20576F726C64
WHERE imagem IS NULL
GO


