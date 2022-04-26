package com.example.plugins

import Database.DAOFacadeImpl
import Database.initDatabase
import Entities.User
import com.apurebase.kgraphql.GraphQL
import io.ktor.application.*
import org.jetbrains.exposed.sql.select

fun Application.configureKGrapQL(dbfacade: DAOFacadeImpl) {
    install(GraphQL) {
        playground = true
        schema {
            query("hello") {
                resolver { -> "World!" }
            }
            query("user") {
                resolver() { id: Int ->
                    dbfacade.getUserById(id)
                }
            }
            type<User>()
        }
    }
}