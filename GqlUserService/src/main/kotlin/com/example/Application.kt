package com.example

import Database.DAOFacadeImpl
import Database.initDatabase
import io.ktor.application.*
import com.example.plugins.*

fun main(args: Array<String>): Unit =
    io.ktor.server.netty.EngineMain.main(args)

@Suppress("unused") // application.conf references the main function. This annotation prevents the IDE from marking it as unused.
fun Application.module() {
    initDatabase(environment.config)
    val dbfacade = DAOFacadeImpl()
    configureKGrapQL(dbfacade)
//    configureRouting()
}
