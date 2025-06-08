INSERT INTO usuarios (
    nome, email, senha_hash, senha_salt, taxa_corretagem, perfil_investidor, data_criacao, data_atualizacao
) VALUES
('João Silva', 'joao@email.com', RANDOM_BYTES(64), RANDOM_BYTES(64), 0.0050, 1, NOW(), NOW()),
('Maria Souza', 'maria@email.com', RANDOM_BYTES(64), RANDOM_BYTES(64), 0.0045, 2, NOW(), NOW());

INSERT INTO ativos (
    codigo_ativo, nome_ativo, risco_ativo, data_criacao, data_atualizacao
) VALUES
('BBDC4', 'Bradesco PN', 2, NOW(), NOW()),
('ITSA4', 'Itaúsa PN', 2, NOW(), NOW()),
('MGLU3', 'Magazine Luiza ON', 2, NOW(), NOW()),
('PETR4', 'Petrobras PN', 2, NOW(), NOW()),
('VALE3', 'Vale ON', 2, NOW(), NOW());

INSERT INTO cotacoes (
    id_ativo, preco_unitario, data_hora_cotacao, data_criacao, data_atualizacao
) VALUES
((SELECT id_ativo FROM ativos WHERE codigo_ativo = 'BBDC4'), 15.96, '2025-06-06 17:07:49', NOW(), NOW()),
((SELECT id_ativo FROM ativos WHERE codigo_ativo = 'ITSA4'), 10.81, '2025-06-06 17:07:38', NOW(), NOW()),
((SELECT id_ativo FROM ativos WHERE codigo_ativo = 'MGLU3'), 9.50, '2025-06-06 17:07:40', NOW(), NOW()),
((SELECT id_ativo FROM ativos WHERE codigo_ativo = 'PETR4'), 29.63, '2025-06-06 17:28:17', NOW(), NOW()),
((SELECT id_ativo FROM ativos WHERE codigo_ativo = 'VALE3'), 52.98, '2025-06-06 17:07:31', NOW(), NOW());

INSERT INTO operacoes (
    id_usuario, id_ativo, quantidade, preco_unitario, tipo_operacao, valor_corretagem, data_hora_operacao, data_criacao, data_atualizacao
) VALUES
(1, (SELECT id_ativo FROM ativos WHERE codigo_ativo = 'PETR4'), 100, 34.50, 0, 1.50, '2024-01-10 11:00:00', NOW(), NOW()),
(1, (SELECT id_ativo FROM ativos WHERE codigo_ativo = 'ITSA4'), 50, 30.00, 1, 0.75, '2024-01-11 11:00:00', NOW(), NOW()),
(2, (SELECT id_ativo FROM ativos WHERE codigo_ativo = 'ITSA4'), 200, 29.75, 0, 2.00, '2024-01-12 11:00:00', NOW(), NOW());

INSERT INTO posicoes (
    id_usuario, id_ativo, quantidade, preco_medio, lucro_prejuizo_atual, data_criacao, data_atualizacao
) VALUES
(1, (SELECT id_ativo FROM ativos WHERE codigo_ativo = 'PETR4'), 100, 34.50, 0.50, NOW(), NOW()),
(1, (SELECT id_ativo FROM ativos WHERE codigo_ativo = 'ITSA4'), 50, 30.00, -0.20, NOW(), NOW()),
(2, (SELECT id_ativo FROM ativos WHERE codigo_ativo = 'ITSA4'), 200, 29.75, 0.05, NOW(), NOW());
