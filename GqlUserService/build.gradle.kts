import org.jetbrains.kotlin.gradle.tasks.KotlinCompile
import com.google.protobuf.gradle.*

val ktor_version: String by project
val kotlin_version: String by project
val logback_version: String by project
val KGraphQLVersion: String by project
val exposedVersion: String by project
val flywayVersion: String by project
val hikariCPVersion: String by project
val mssqlDriverVersion: String by project

plugins {
    application
    kotlin("jvm") version "1.6.10"
    id("com.github.johnrengelman.shadow") version "7.1.2"
    id("com.google.protobuf") version "0.8.18"
}

group = "com.example"
version = "0.0.1"
application {
    mainClass.set("io.ktor.server.netty.EngineMain")

    val isDevelopment: Boolean = project.ext.has("development")
    applicationDefaultJvmArgs = listOf("-Dio.ktor.development=$isDevelopment")
}

repositories {
    mavenCentral()
    google()
    maven {
        url = uri("https://maven.pkg.jetbrains.space/public/p/ktor/eap")
        name = "Ktor EAP"
    }
}

sourceSets {
    main {
        proto {
            srcDir("src/main/protobuf")
        }
    }
}



dependencies {
    implementation("io.ktor:ktor-server-core:$ktor_version")
    implementation("io.ktor:ktor-server-netty:$ktor_version")
    implementation("ch.qos.logback:logback-classic:$logback_version")
    implementation("com.apurebase:kgraphql-ktor:$KGraphQLVersion")                       //GraphQL
    implementation("org.jetbrains.exposed:exposed-core:$exposedVersion")                 //Exposed
    implementation("org.jetbrains.exposed:exposed-dao:$exposedVersion")
    implementation("org.jetbrains.exposed:exposed-jdbc:$exposedVersion")
    implementation("org.flywaydb:flyway-core:$flywayVersion")                                    //FlyWay
    implementation("com.zaxxer:HikariCP:$hikariCPVersion")                                     //HikariCP
    implementation("com.microsoft.sqlserver:mssql-jdbc:$mssqlDriverVersion")                     //MSSQL-Driver
    implementation("org.postgresql:postgresql:42.3.6")                  //PostgreSQL
    implementation("com.google.protobuf:protobuf-kotlin:3.19.4")
    api("io.grpc:grpc-protobuf:1.44.0")
    api("com.google.protobuf:protobuf-java-util:3.19.4")
    api("com.google.protobuf:protobuf-kotlin:3.19.4")
    api("io.grpc:grpc-kotlin-stub:1.2.1")
    api("io.grpc:grpc-stub:1.44.0")
    runtimeOnly("io.grpc:grpc-netty:1.44.0")
    testImplementation("io.ktor:ktor-server-tests:$ktor_version")
    testImplementation("org.jetbrains.kotlin:kotlin-test-junit:$kotlin_version")
}

tasks.test {
    useJUnitPlatform()
}

tasks.withType<KotlinCompile> {
    kotlinOptions.jvmTarget = "1.8"
}

protobuf {
    protoc {
        artifact = "com.google.protobuf:protoc:3.19.4"
    }
    plugins {
        id("grpc") {
            artifact = "io.grpc:protoc-gen-grpc-java:1.44.0"
        }
        id("grpckt") {
            artifact = "io.grpc:protoc-gen-grpc-kotlin:1.2.1:jdk7@jar"
        }
    }


    generateProtoTasks {
        generatedFilesBaseDir = "src"
        all().forEach {
            it.plugins {
                id("grpc")
                id("grpckt")
            }
            it.builtins {
                id("kotlin")
            }
        }
    }
}
