package com.example.grpc

import io.grpc.Status

class ServerError(code: Int) : io.grpc.StatusRuntimeException(Status.fromThrowable(Throwable(code.toString())).withDescription(code.toString()))

const val noCityDefinedInQuery = 2000 // не отправлен город в запрос получения идентификатор локации
const val noCurrentCompoundFound = 2001 // не найдена запись по общему идентификатору текущей локации
const val noWantedCompoundFound = 2002 // не найдена запись по общему идентификатору желаемой локации

const val serverLogicError = 1000 // ошибка логики сервера