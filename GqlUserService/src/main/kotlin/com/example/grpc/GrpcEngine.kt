package com.example.grpc

import io.grpc.Server
import io.grpc.ServerBuilder
import io.ktor.server.engine.*
import io.ktor.application.ApplicationStopPreparing
import io.ktor.server.engine.ApplicationEngine
import io.ktor.server.engine.ApplicationEngineEnvironment
import io.ktor.server.engine.ApplicationEngineFactory
import io.ktor.server.engine.BaseApplicationEngine
import java.util.concurrent.TimeUnit

@EngineAPI
object GrpcEngine : ApplicationEngineFactory<GrpcApplicationEngine, GrpcApplicationEngine.Configuration> {
    override fun create(
        environment: ApplicationEngineEnvironment,
        configure: GrpcApplicationEngine.Configuration.() -> Unit
    ): GrpcApplicationEngine {
        return GrpcApplicationEngine(environment, configure)
    }
}

@EngineAPI
class GrpcApplicationEngine(
    environment: ApplicationEngineEnvironment,
    val configure: GrpcApplicationEngine.Configuration.() -> Unit = {}
) : BaseApplicationEngine(environment) {

    class Configuration : BaseApplicationEngine.Configuration() {
        var port: Int = 6565

        var serverConfigurer: ServerBuilder<*>.() -> Unit = {}
//        ServerBootstrap.() -> Unit = {}
    }

    private val configuration = GrpcApplicationEngine.Configuration().apply(configure)
    private var server: Server? = null

    override fun start(wait: Boolean): ApplicationEngine {
        server = ServerBuilder
            .forPort(configuration.port)
            .apply(configuration.serverConfigurer)
            .build()

        server!!.start()

        if (wait) {
            server!!.awaitTermination()
//            server.shutdown()
        }

        return this
    }

    override fun stop(gracePeriodMillis: Long, timeoutMillis: Long) {
        environment.monitor.raise(ApplicationStopPreparing, environment)

        with(server){
            this?.let {
                awaitTermination(gracePeriodMillis, TimeUnit.MILLISECONDS)
                shutdownNow()
            }
        }
    }
}