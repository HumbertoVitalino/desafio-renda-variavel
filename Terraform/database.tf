resource "aws_db_subnet_group" "default" {
  name       = "main-subnet-group-desafio"
  subnet_ids = data.aws_subnets.default.ids
}

resource "aws_db_instance" "default_db" {
  identifier             = "desafio-itau-db"
  allocated_storage      = 20
  engine                 = "mysql"
  engine_version         = "8.0"
  instance_class         = "db.t3.micro"
  db_name                = "investimentos_rv"
  username               = "admin"
  password               = "!1234567"
  db_subnet_group_name   = aws_db_subnet_group.default.name
  vpc_security_group_ids = [aws_security_group.db_sg.id]
  skip_final_snapshot    = true
  publicly_accessible    = true
}
