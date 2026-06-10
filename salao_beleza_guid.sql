CREATE DATABASE OrBlancDB;
GO

USE OrBlancDB;
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
    ativo           BIT              NOT NULL DEFAULT 1,
    senha           VARBINARY(32)    NOT NULL,

    CONSTRAINT PK_Profissional PRIMARY KEY (id_profissional)
);
GO

CREATE TABLE Servico (
    id_servico  INT            IDENTITY(1,1) NOT NULL,
    nome        VARCHAR(100)   NOT NULL,
    descricao   VARCHAR(255)   NULL,
    valor       DECIMAL(10, 2) NOT NULL,
    ativo       BIT            NOT NULL DEFAULT 1,

    CONSTRAINT PK_Servico       PRIMARY KEY (id_servico),
    CONSTRAINT CK_Servico_Valor CHECK (valor >= 0)
);
GO

CREATE TABLE Agendamento (
    id_agendamento   INT              IDENTITY(1,1)    NOT NULL,
    id_cliente       UNIQUEIDENTIFIER NOT NULL,
    id_profissional  UNIQUEIDENTIFIER NOT NULL,
    id_servico       INT              NOT NULL,
    data_hora_inicio DATETIME         NOT NULL,
    data_hora_fim    DATETIME         NOT NULL,
    status           VARCHAR(20)      NOT NULL DEFAULT 'Agendado',
    observacao       VARCHAR(500)     NULL,

    CONSTRAINT PK_Agendamento PRIMARY KEY (id_agendamento),

    CONSTRAINT FK_Agendamento_Cliente
        FOREIGN KEY (id_cliente) REFERENCES Cliente(id_cliente),

    CONSTRAINT FK_Agendamento_Profissional
        FOREIGN KEY (id_profissional) REFERENCES Profissional(id_profissional),

    CONSTRAINT FK_Agendamento_Servico
        FOREIGN KEY (id_servico) REFERENCES Servico(id_servico),

    CONSTRAINT CK_Agendamento_Horario
        CHECK (data_hora_fim > data_hora_inicio),

    CONSTRAINT CK_Agendamento_Status
        CHECK (status IN ('Agendado', 'Confirmado', 'Concluído', 'Cancelado'))
);
GO


CREATE INDEX IX_Agendamento_Cliente
    ON Agendamento (id_cliente);
GO

CREATE INDEX IX_Agendamento_Profissional
    ON Agendamento (id_profissional);
GO

CREATE INDEX IX_Agendamento_DataHora
    ON Agendamento (data_hora_inicio);
GO


DECLARE @cli1 UNIQUEIDENTIFIER = NEWID();
DECLARE @cli2 UNIQUEIDENTIFIER = NEWID();
DECLARE @cli3 UNIQUEIDENTIFIER = NEWID();
DECLARE @cli4 UNIQUEIDENTIFIER = NEWID();
DECLARE @cli5 UNIQUEIDENTIFIER = NEWID();

INSERT INTO Cliente (id_cliente, nome, telefone, email, senha) VALUES
    (@cli1, 'Ana Paula Silva',  '11987654321', 'ana.paula@email.com',    HASHBYTES('SHA2_256', 'ana123')),
    (@cli2, 'Beatriz Souza',    '11976543210', 'beatriz.souza@email.com', HASHBYTES('SHA2_256', 'bea123')),
    (@cli3, 'Carlos Oliveira',  '11965432109', 'carlos.oliveira@email.com', HASHBYTES('SHA2_256', 'car123')),
    (@cli4, 'Diana Lima',       '11954321098', 'diana.lima@email.com',   HASHBYTES('SHA2_256', 'dia123')),
    (@cli5, 'Eduardo Santos',   '11943210987', 'eduardo.santos@email.com', HASHBYTES('SHA2_256', 'edu123'));

DECLARE @pro1 UNIQUEIDENTIFIER = NEWID();
DECLARE @pro2 UNIQUEIDENTIFIER = NEWID();
DECLARE @pro3 UNIQUEIDENTIFIER = NEWID();
DECLARE @pro4 UNIQUEIDENTIFIER = NEWID();
DECLARE @pro5 UNIQUEIDENTIFIER = NEWID();

INSERT INTO Profissional (id_profissional, nome, especialidade, telefone, ativo, senha) VALUES
    (@pro1, 'Fernanda Costa',   'Cabelereira', '11911112222', 1, HASHBYTES('SHA2_256', 'fer123')),
    (@pro2, 'Gabriela Mendes',  'Manicure',    '11922223333', 1, HASHBYTES('SHA2_256', 'gab123')),
    (@pro3, 'Helena Rodrigues', 'Esteticista', '11933334444', 1, HASHBYTES('SHA2_256', 'hel123')),
    (@pro4, 'Igor Ferreira',    'Cabelereiro', '11944445555', 1, HASHBYTES('SHA2_256', 'igo123')),
    (@pro5, 'Juliana Martins',  'Sobrancelha', '11955556666', 0, HASHBYTES('SHA2_256', 'jul123'));

INSERT INTO Servico (nome, descricao, valor, ativo) VALUES
    ('Corte Feminino',     'Corte de cabelo feminino',            60.00, 1),
    ('Corte Masculino',    'Corte de cabelo masculino',           40.00, 1),
    ('Coloração',          'Tintura completa do cabelo',         150.00, 1),
    ('Escova',             'Escova modeladora',                   50.00, 1),
    ('Manicure',           'Unhas das mãos com esmalte comum',    30.00, 1),
    ('Pedicure',           'Unhas dos pés com esmalte comum',     35.00, 1),
    ('Design Sobrancelha', 'Design e depilação de sobrancelha',   25.00, 1),
    ('Limpeza de Pele',    'Limpeza profunda e hidratação',      120.00, 1);

INSERT INTO Agendamento (id_cliente, id_profissional, id_servico, data_hora_inicio, data_hora_fim, status, observacao) VALUES
    (@cli1, @pro1, 1, '2026-06-10 09:00', '2026-06-10 10:00', 'Confirmado', 'Cliente prefere franja lateral'),
    (@cli2, @pro2, 5, '2026-06-10 09:00', '2026-06-10 09:45', 'Agendado',   NULL),
    (@cli3, @pro1, 4, '2026-06-10 10:30', '2026-06-10 11:30', 'Agendado',   NULL),
    (@cli4, @pro3, 8, '2026-06-10 11:00', '2026-06-10 12:00', 'Agendado',   'Pele sensível'),
    (@cli5, @pro4, 2, '2026-06-10 14:00', '2026-06-10 14:45', 'Agendado',   NULL),
    (@cli1, @pro2, 6, '2026-06-11 10:00', '2026-06-11 10:50', 'Agendado',   NULL),
    (@cli2, @pro1, 3, '2026-06-11 09:00', '2026-06-11 11:00', 'Confirmado', 'Cor desejada: castanho escuro'),
    (@cli3, @pro4, 2, '2026-06-11 15:00', '2026-06-11 15:45', 'Cancelado',  'Cliente desmarcou');
GO


CREATE VIEW VW_AgendaCompleta AS
SELECT
    a.id_agendamento,
    c.nome                                                AS nome_cliente,
    c.telefone                                            AS telefone_cliente,
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
    Agendamento  a
    INNER JOIN Cliente      c ON c.id_cliente      = a.id_cliente
    INNER JOIN Profissional p ON p.id_profissional = a.id_profissional
    INNER JOIN Servico      s ON s.id_servico      = a.id_servico;
GO


CREATE TRIGGER TRG_VerificaConflito
ON Agendamento
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
        @id_agendamento  = id_agendamento,
        @id_profissional = id_profissional,
        @data_inicio     = data_hora_inicio,
        @data_fim        = data_hora_fim
    FROM inserted;

    SELECT @conflitos = COUNT(*)
    FROM Agendamento
    WHERE
        id_profissional = @id_profissional
        AND id_agendamento <> @id_agendamento
        AND status NOT IN ('Cancelado')
        AND @data_inicio < data_hora_fim
        AND @data_fim    > data_hora_inicio;

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
    BEGIN
        PRINT 'ERRO: Cliente não encontrado.';
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM Profissional WHERE id_profissional = @id_profissional AND ativo = 1)
    BEGIN
        PRINT 'ERRO: Profissional não encontrado ou está inativo.';
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM Servico WHERE id_servico = @id_servico AND ativo = 1)
    BEGIN
        PRINT 'ERRO: Serviço não encontrado ou está inativo.';
        RETURN;
    END

    IF @data_hora_fim <= @data_hora_inicio
    BEGIN
        PRINT 'ERRO: O horário de término deve ser maior que o horário de início.';
        RETURN;
    END

    INSERT INTO Agendamento (id_cliente, id_profissional, id_servico, data_hora_inicio, data_hora_fim, status, observacao)
    VALUES (@id_cliente, @id_profissional, @id_servico, @data_hora_inicio, @data_hora_fim, 'Agendado', @observacao);

    PRINT 'Agendamento realizado com sucesso! ID: ' + CAST(SCOPE_IDENTITY() AS VARCHAR);
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

select * from Cliente

select * from Profissional

-- Remove as FKs antigas
ALTER TABLE Agendamento DROP CONSTRAINT FK_Agendamento_Cliente;
ALTER TABLE Agendamento DROP CONSTRAINT FK_Agendamento_Profissional;
ALTER TABLE Agendamento DROP CONSTRAINT FK_Agendamento_Servico;
GO

-- Recria só Cliente com CASCADE (hard delete faz sentido)
ALTER TABLE Agendamento
    ADD CONSTRAINT FK_Agendamento_Cliente
    FOREIGN KEY (id_cliente) REFERENCES Cliente(id_cliente)
    ON DELETE CASCADE;

-- Profissional e Serviço SEM cascade (usaremos soft delete)
ALTER TABLE Agendamento
    ADD CONSTRAINT FK_Agendamento_Profissional
    FOREIGN KEY (id_profissional) REFERENCES Profissional(id_profissional)
    ON DELETE NO ACTION;

ALTER TABLE Agendamento
    ADD CONSTRAINT FK_Agendamento_Servico
    FOREIGN KEY (id_servico) REFERENCES Servico(id_servico)
    ON DELETE NO ACTION;
GO

ALTER TABLE Profissional
    ADD email VARCHAR(100);

