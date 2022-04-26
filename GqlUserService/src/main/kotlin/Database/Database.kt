package Database

import Entities.User
import Entities.Users
import Entities.Users.description
import com.zaxxer.hikari.HikariConfig
import com.zaxxer.hikari.HikariDataSource
import io.ktor.config.*
import kotlinx.coroutines.Dispatchers
import org.flywaydb.core.Flyway
import org.jetbrains.exposed.sql.*
import org.jetbrains.exposed.sql.SqlExpressionBuilder.eq
import org.jetbrains.exposed.sql.transactions.experimental.newSuspendedTransaction
import org.jetbrains.exposed.sql.transactions.transaction

fun initDatabase(config: ApplicationConfig) {
    val driverClassName = config.property("storage.driverClassName").getString()
    val jdbcURL = config.property("storage.jdbcURL").getString()
    val dbUsername = config.property("storage.username").getString()
    val dbPassword = config.property("storage.password").getString()
    val dataSource = createHikariDataSource(
        url = jdbcURL,
        driver = driverClassName,
        user = dbUsername,
        dbPassword = dbPassword
    )

//    val flyway = Flyway.configure().dataSource(dataSource).load()
//    flyway.migrate()

    Database.connect(
        dataSource
    )

    transaction {
        addLogger(StdOutSqlLogger)
        SchemaUtils.create(Users)

        Users.insert {
            it[username] = "Andrey"
            it[description] = "Andrey description"
        }

        Users.insert {
            it[username] = "Vladimir"
            it[description] = "Vladimir description"
        }

        Users.insert {
            it[username] = "Grigory"
            it[description] = "Grigory description"
        }
    }
}

private fun createHikariDataSource(
    url: String,
    driver: String,
    user: String,
    dbPassword: String
) = HikariDataSource(HikariConfig().apply {
    username = user
    password = dbPassword
    driverClassName = driver
    jdbcUrl = url
    maximumPoolSize = 3
    isAutoCommit = false
    transactionIsolation = "TRANSACTION_REPEATABLE_READ"
    validate()
})

interface DAOFacade {
    suspend fun getUserById(id: Int): User
}

class DAOFacadeImpl : DAOFacade {
    override suspend fun getUserById(id: Int): User =
        dbQuery { Users.select { Users.id.eq(id) }.map { User.fromRow(it) }.single() }
}

private suspend fun <T> dbQuery(block: () -> T): T = newSuspendedTransaction(Dispatchers.IO) { block() }