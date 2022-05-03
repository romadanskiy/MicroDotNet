package com.example.plugins

import Database.DAOFacadeImpl
import Database.initDatabase
import Entities.User
import com.apurebase.kgraphql.GraphQL
import io.ktor.application.*
import org.jetbrains.exposed.sql.select
import java.util.*

fun Application.configureKGrapQL(dbfacade: DAOFacadeImpl) {
    install(GraphQL) {
        playground = true
        schema {
            query("hello") {
                resolver { -> "World!" }
            }
            query("user") {
                resolver() { id: String ->
                    dbfacade.getUserById(UUID.fromString(id))
                }
            }
            type<User>()
        }
    }
}