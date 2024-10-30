#!/bin/sh

# Função para verificar se o serviço está pronto
wait_for_service() {
  host="$1"
  port="$2"
  shift 2
  while ! nc -z "$host" "$port"; do
    echo "Aguardando $host:$port..."
    sleep 3
  done
  echo "$host:$port está disponível!"
}

# Espera pelo banco de dados e RabbitMQ
#wait_for_service consolidado_db 5432
wait_for_service rabbitmq 5672

# Inicia a aplicação depois que os serviços estão disponíveis
exec "$@"
