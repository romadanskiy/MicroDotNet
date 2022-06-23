package com.example.scanner.presentation.viewmodels

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel

open class LoadedViewModel(): ViewModel() {
    val loadingMutable: MutableLiveData<Boolean> = MutableLiveData(true);
    val loading: LiveData<Boolean> = loadingMutable

    fun setLoading(isLoading: Boolean){
        loadingMutable.postValue(isLoading)
    }

    open fun clear(){
        setLoading(true)
    }
}