package com.example

import Database.DAOFacadeImpl
import Database.initDatabase
import com.example.di.appModule
import com.example.di.get
import com.example.grpc.GrpcEngine
import io.ktor.application.*
import com.example.plugins.*
import com.zaxxer.hikari.HikariConfig
import com.zaxxer.hikari.HikariDataSource
import io.grpc.ServerBuilder
import org.koin.core.KoinApplication
import io.ktor.response.*
import io.ktor.routing.*
import io.ktor.server.engine.*
import io.ktor.server.netty.*
//import RatingGRPC
import ratingReply
//import sun.awt.geom.Crossings.debug
import java.util.*


private class RatingGRPCService : RatingGRPCGrpcKt.RatingGRPCCoroutineImplBase() {
    override suspend fun getRatingByUserId(request: RatingGRPCOuterClass.Request): RatingGRPCOuterClass.RatingReply {
        //ToDo как добавится рейтнг, вставить сюда
        val dbfacade = DAOFacadeImpl()
        val rating_Reply = dbfacade.getUserById(UUID.fromString(request.userId))

        return ratingReply{ rating = rating_Reply.toString() }
    }
}

@EngineAPI
fun main(args: Array<String>): Unit{
//    embeddedServer(GrpcEngine, port = 50051, ServerConfigurer = )
    val k = initDI()
    initServer(k)
}

@EngineAPI
private fun initServer(k: KoinApplication) {
    embeddedServer(GrpcEngine, configure = {
        port = 7777
        serverConfigurer = {
            addService(k.get<RatingGRPCService>())
//            addService(k.get<RestBindable>())
//            addService(k.get<CommentBindable>())
//            addService(k.get<UserBindable>())
//            addService(k.get<ResponseBindable>())
        }
    }) {
        initDatabase(environment.config)

//        debug = this.environment.developmentMode
    }.start(wait = true)
}

private fun initDI(): KoinApplication = KoinApplication.init().apply {
    modules(appModule)
}
//@Suppress("unused") // application.conf references the main function. This annotation prevents the IDE from marking it as unused.
//fun Application.module() {
//    initDatabase(environment.config)
//    val dbfacade = DAOFacadeImpl()
//    configureKGrapQL(dbfacade)
//    //configureRouting()
//}
