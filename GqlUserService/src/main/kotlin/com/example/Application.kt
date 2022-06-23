package com.example

import Database.DAOFacadeImpl
import Database.initDatabase
import io.ktor.application.*
import com.example.plugins.*
import io.grpc.ServerBuilder
import ratingReply

private class RatingGRPCService : RatingGRPCGrpcKt.RatingGRPCCoroutineImplBase() {
    override suspend fun getRatingByUserId(request: RatingGRPCOuterClass.Request): RatingGRPCOuterClass.RatingReply {
        //ToDo как добавится рейтнг, вставить сюда
        return ratingReply{ rating = "11" }
    }
}

fun main(args: Array<String>): Unit{
    val server = ServerBuilder
        .forPort(50071)
        .addService(RatingGRPCService())
        .build()
    server.start()
    server.awaitTermination()
    io.ktor.server.netty.EngineMain.main(args)
}

@Suppress("unused")
fun Application.module() {
    initDatabase(environment.config)
    val dbfacade = DAOFacadeImpl()
    configureKGrapQL(dbfacade)
    configureRouting()
}
