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
    kotlin("jvm") version "1.6.21"
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
    implementation("org.postgresql:postgresql:42.3.4")                                  //PostgreSQL
    testImplementation("io.ktor:ktor-server-tests:$ktor_version")
    testImplementation("org.jetbrains.kotlin:kotlin-test-junit:$kotlin_version")
}