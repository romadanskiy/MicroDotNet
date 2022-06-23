package com.example.di

import org.koin.core.KoinApplication
import org.koin.core.parameter.ParametersDefinition
import org.koin.core.qualifier.Qualifier


inline fun <reified T : Any> KoinApplication.get(
    qualifier: Qualifier? = null,
    mode: LazyThreadSafetyMode = LazyThreadSafetyMode.SYNCHRONIZED,
    noinline parameters: ParametersDefinition? = null
) = koin.inject<T>(qualifier, mode, parameters).value