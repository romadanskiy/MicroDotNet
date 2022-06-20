package com.example

import Database.DAOFacadeImpl
import Database.initDatabase
import io.ktor.application.*
import com.example.plugins.*
import io.grpc.ServerBuilder
import registrationResult

private class RegistrationService : RegistrationServiceGrpcKt.RegistrationServiceCoroutineImplBase() {
    override suspend fun register(request: Register.User): Register.RegistrationResult {
        print("Registering user ${request.lastname} ${request.firstname} ${request.middlename}, age: ${request.age}, gender: ${request.gender.name}")
        return registrationResult { succeeded=true }
    }
}

fun main(args: Array<String>): Unit{
    io.ktor.server.netty.EngineMain.main(args)
    val port = System.getenv("PORT")?.toInt() ?: 50051
    val server = ServerBuilder
        .forPort(port)
        .addService(RegistrationService())
        .build()
    server.start()
    Runtime.getRuntime().addShutdownHook(Thread {
        println("Shutdown server")
        server.shutdown()
    })
    //wait for connection until shutdown
    server.awaitTermination()
}

@Suppress("unused") // application.conf references the main function. This annotation prevents the IDE from marking it as unused.
fun Application.module() {
    initDatabase(environment.config)
    val dbfacade = DAOFacadeImpl()
    configureKGrapQL(dbfacade)
//    configureRouting()
}
