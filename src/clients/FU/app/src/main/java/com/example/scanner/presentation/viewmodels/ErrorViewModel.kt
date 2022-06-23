package com.example.scanner.presentation.viewmodels

import androidx.fragment.app.Fragment
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ReportFragment
import androidx.lifecycle.ViewModel
import com.example.scanner.ApiFailedRequestResult
import com.example.scanner.ApiRequestResult
import com.example.scanner.domain.models.Barcode

class ErrorViewModel: ViewModel() {
    private val mutableCode: MutableLiveData<Int> = MutableLiveData()
    private val mutableMessages: MutableLiveData<List<String>> = MutableLiveData()
    private val mutableBackFragment: MutableLiveData<Fragment> = MutableLiveData()

    val code : LiveData<Int> = mutableCode
    val messages: LiveData<List<String>> = mutableMessages
    val fragment: LiveData<Fragment> = mutableBackFragment

    fun handleRequestError(request: ApiFailedRequestResult, backFragment: Fragment?){
        mutableCode.postValue(request.code)
        mutableMessages.postValue(request.errorMessages)
        mutableBackFragment.postValue(backFragment)
    }
}