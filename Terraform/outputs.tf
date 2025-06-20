output "rds_endpoint" {
  description = "O endereço de conexão para a instância RDS"
  value       = aws_db_instance.default_db.endpoint
}

output "rds_port" {
  description = "A porta da instância RDS"
  value       = aws_db_instance.default_db.port
}