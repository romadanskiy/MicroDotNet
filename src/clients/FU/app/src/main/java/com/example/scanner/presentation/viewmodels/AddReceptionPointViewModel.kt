package com.example.scanner.presentation.viewmodels

import android.content.Context
import android.graphics.drawable.BitmapDrawable
import android.widget.ImageView
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.viewModelScope
import com.example.scanner.ApiRequestResult
import com.example.scanner.domain.models.GarbageCategory
import com.example.scanner.domain.models.GarbageInfo
import com.example.scanner.domain.models.ReceptionPoint
import com.example.scanner.domain.usecase.AddReceptionPointUseCase
import com.example.scanner.presentation.utils.BitmapToFileConverter
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import java.io.File

class AddReceptionPointViewModel(val addReceptionPointUseCase: AddReceptionPointUseCase): LoadedViewModel() {

    private val mutablePointInfo: MutableLiveData<ReceptionPoint> = MutableLiveData()
    private val mutableResult: MutableLiveData<ApiRequestResult<String>> = MutableLiveData()
    val pontInfo: LiveData<ReceptionPoint> = mutablePointInfo
    val result: LiveData<ApiRequestResult<String>> = mutableResult

    fun addPointInfo(){
        viewModelScope.launch(Dispatchers.IO) {
            setLoading(true)
            val requestResult = addReceptionPointUseCase.execute(mutablePointInfo.value!!);

            val apiRequestResult = ApiRequestResult(
                requestResult.responseCode,
                requestResult.success,
                requestResult.data,
                requestResult.messages
            )
            mutableResult.postValue(apiRequestResult)
        }
    }

    fun setPointInfo(garbageInfo: ReceptionPoint){
        mutablePointInfo.value = garbageInfo
    }



    fun setResult(result: ApiRequestResult<String>?){
        mutableResult.postValue(result)
    }

    override fun clear() {
        super.clear()
        loadingMutable.postValue(false)
        mutablePointInfo.postValue(null);
        mutableResult.postValue(null)
    }
}