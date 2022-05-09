package com.example.scanner.di

import com.example.scanner.data.repository.GarbageRepositoryImpl
import com.example.scanner.data.storage.GarbageStorage
import com.example.scanner.data.storage.retrofit2.GarbageStorageImpl
import com.example.scanner.domain.repository.GarbageRepository
import org.koin.dsl.module

val dataModule = module {

    single<GarbageRepository> {
        GarbageRepositoryImpl(garbageStorage = get())
    }

    single<GarbageStorage> {
        GarbageStorageImpl()
    }
}