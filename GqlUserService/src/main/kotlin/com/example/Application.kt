package com.example

import Database.DAOFacadeImpl
import Database.initDatabase
import io.ktor.application.*
import com.example.plugins.*
import io.grpc.ServerBuilder
import ratingReply

private class RatingGRPCService : RatingGRPCGrpcKt.RatingGRPCCoroutineImplBase() {
    override suspend fun getRatingByUserId(request: RatingGRPCOuterClass.Request): RatingGRPCOuterClass.RatingReply {
        return ratingReply{ rating = "11" }
    }
}

fun main(args: Array<String>): Unit{
    io.ktor.server.netty.EngineMain.main(args)
    val server = ServerBuilder
        .forPort(8089)
        .addService(RatingGRPCService())
        .build()
    server.start()
}

@Suppress("unused") // application.conf references the main function. This annotation prevents the IDE from marking it as unused.
fun Application.module() {
    initDatabase(environment.config)
    val dbfacade = DAOFacadeImpl()
    configureKGrapQL(dbfacade)
    configureRouting()
}
