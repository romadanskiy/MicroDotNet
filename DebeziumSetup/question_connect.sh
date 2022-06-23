curl -H 'Content-Type: application/json' debezium:8083/connectors --data '
      {
        "name": "question-connector",
        "config": {
          "connector.class": "io.debezium.connector.postgresql.PostgresConnector",
          "plugin.name": "pgoutput",
          "database.hostname": "postgres",
          "database.port": "5432",
          "database.user": "postgres",
          "database.password": "postgres",
          "database.dbname": "ruflow",
          "database.server.name": "postgres",
          "table.include.list": "public.questions"
        }
      }'