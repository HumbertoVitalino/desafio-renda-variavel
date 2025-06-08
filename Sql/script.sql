CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `ativos` (
    `id_ativo` int NOT NULL AUTO_INCREMENT,
    `codigo_ativo` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
    `nome_ativo` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `data_criacao` datetime(6) NOT NULL,
    `data_atualizacao` datetime(6) NOT NULL,
    CONSTRAINT `PK_ativos` PRIMARY KEY (`id_ativo`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `usuarios` (
    `id_usuario` int NOT NULL AUTO_INCREMENT,
    `nome` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `email` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `senha_hash` longblob NOT NULL,
    `senha_salt` longblob NOT NULL,
    `taxa_corretagem` decimal(5,4) NOT NULL,
    `data_criacao` datetime(6) NOT NULL,
    `data_atualizacao` datetime(6) NOT NULL,
    CONSTRAINT `PK_usuarios` PRIMARY KEY (`id_usuario`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `cotacoes` (
    `id_cotacao` int NOT NULL AUTO_INCREMENT,
    `id_ativo` int NOT NULL,
    `preco_unitario` decimal(18,8) NOT NULL,
    `data_hora_cotacao` datetime(6) NOT NULL,
    `data_criacao` datetime(6) NOT NULL,
    `data_atualizacao` datetime(6) NOT NULL,
    CONSTRAINT `PK_cotacoes` PRIMARY KEY (`id_cotacao`),
    CONSTRAINT `FK_cotacoes_ativos_id_ativo` FOREIGN KEY (`id_ativo`) REFERENCES `ativos` (`id_ativo`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `operacoes` (
    `id_operacao` int NOT NULL AUTO_INCREMENT,
    `id_usuario` int NOT NULL,
    `id_ativo` int NOT NULL,
    `quantidade` int NOT NULL,
    `preco_unitario` decimal(18,8) NOT NULL,
    `tipo_operacao` int NOT NULL,
    `valor_corretagem` decimal(18,8) NOT NULL,
    `data_hora_operacao` datetime(6) NOT NULL,
    `data_criacao` datetime(6) NOT NULL,
    `data_atualizacao` datetime(6) NOT NULL,
    CONSTRAINT `PK_operacoes` PRIMARY KEY (`id_operacao`),
    CONSTRAINT `FK_operacoes_ativos_id_ativo` FOREIGN KEY (`id_ativo`) REFERENCES `ativos` (`id_ativo`) ON DELETE RESTRICT,
    CONSTRAINT `FK_operacoes_usuarios_id_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id_usuario`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `posicoes` (
    `id_posicao` int NOT NULL AUTO_INCREMENT,
    `id_usuario` int NOT NULL,
    `id_ativo` int NOT NULL,
    `quantidade` int NOT NULL,
    `preco_medio` decimal(18,8) NOT NULL,
    `lucro_prejuizo_atual` decimal(18,8) NOT NULL,
    `data_criacao` datetime(6) NOT NULL,
    `data_atualizacao` datetime(6) NOT NULL,
    CONSTRAINT `PK_posicoes` PRIMARY KEY (`id_posicao`),
    CONSTRAINT `FK_posicoes_ativos_id_ativo` FOREIGN KEY (`id_ativo`) REFERENCES `ativos` (`id_ativo`) ON DELETE RESTRICT,
    CONSTRAINT `FK_posicoes_usuarios_id_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id_usuario`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE UNIQUE INDEX `IX_ativos_codigo_ativo` ON `ativos` (`codigo_ativo`);

CREATE UNIQUE INDEX `IX_cotacoes_id_ativo_data_hora_cotacao` ON `cotacoes` (`id_ativo`, `data_hora_cotacao`);

CREATE INDEX `IX_operacoes_id_ativo` ON `operacoes` (`id_ativo`);

CREATE INDEX `ix_operacoes_usuario_ativo_data` ON `operacoes` (`id_usuario`, `id_ativo`, `data_hora_operacao`);

CREATE INDEX `IX_posicoes_id_ativo` ON `posicoes` (`id_ativo`);

CREATE UNIQUE INDEX `IX_posicoes_id_usuario_id_ativo` ON `posicoes` (`id_usuario`, `id_ativo`);

CREATE UNIQUE INDEX `IX_usuarios_email` ON `usuarios` (`email`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250605234514_InitialCreate', '8.0.16');

COMMIT;

START TRANSACTION;

ALTER TABLE `usuarios` ADD `perfil_investidor` int NOT NULL DEFAULT 0;

ALTER TABLE `ativos` ADD `risco_ativo` int NOT NULL DEFAULT 0;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250606143145_AddInvestorProfileAndAssetRisk', '8.0.16');

COMMIT;

