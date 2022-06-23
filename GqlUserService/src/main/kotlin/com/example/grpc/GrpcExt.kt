package com.example.grpc

import io.grpc.stub.StreamObserver

fun <T> StreamObserver<T>.dropError(t: Throwable) {
    onError(t)
}

fun <T> StreamObserver<T>.catchLogicError(t: Throwable) {
    onError(
        ServerError(serverLogicError)
    )
}

fun <T> StreamObserver<T>.respond(t: T) {
    onNext(t)
    onCompleted()
}

inline fun <R, O> safeCall(
    request: R,
    observer: StreamObserver<O>,
    block: (R, StreamObserver<O>) -> Unit
) {
    try {
        block(request, observer)
    } catch (e: ServerError) {
        observer.dropError(e)
    } catch (e: Throwable) {
        observer.catchLogicError(e)
    }
}