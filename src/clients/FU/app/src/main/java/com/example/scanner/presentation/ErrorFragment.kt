package com.example.scanner.presentation

import android.R
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import androidx.fragment.app.Fragment
import androidx.lifecycle.LiveData
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModel
import com.example.scanner.databinding.FragmentErrorBinding
import com.example.scanner.presentation.viewmodels.ErrorViewModel
import com.example.scanner.presentation.viewmodels.ScanningResultViewModel
import org.koin.androidx.viewmodel.ext.android.sharedViewModel


class ErrorFragment : Fragment() {

    private lateinit var binding: FragmentErrorBinding
    private val errorViewModel by sharedViewModel<ErrorViewModel>()

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentErrorBinding.inflate(inflater, container, false);
        if(errorViewModel.fragment.value == null){
            binding.backButton.visibility = View.GONE
        }
        else{
            binding.backButton.visibility = View.VISIBLE
            binding.backButton.setOnClickListener {
                val supportFragmentManager = activity?.supportFragmentManager
                supportFragmentManager?.beginTransaction()
                    ?.replace(com.example.scanner.R.id.fragment_container,
                        errorViewModel.fragment.value!!
                    )
                    ?.commit();
            }
        }


        val error =
            Observer<List<String>> { errorList ->
                val arr = activity?.let {
                    ArrayAdapter<String>(
                        it,
                        R.layout.simple_list_item_1,
                        errorList
                    )
                }
                binding.errorList.adapter = arr
            }

        errorViewModel.messages.observe(viewLifecycleOwner, error)
        return binding.root;
    }


}