package com.example.scanner.presentation

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.example.scanner.R
import com.example.scanner.databinding.FragmentScanningResultBinding
import com.example.scanner.domain.models.GarbageCategory

class GarbageCategoriesRecyclerAdapter(private val categories: List<GarbageCategory>) :
    RecyclerView.Adapter<GarbageCategoriesRecyclerAdapter.GarbageCategoriesViewHolder>() {

    private lateinit var binding: FragmentScanningResultBinding

        class GarbageCategoriesViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
            val garbageCategoryNameTextView: TextView = itemView.findViewById(R.id.garbageCategoryName)

        }

        override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): GarbageCategoriesViewHolder {
            val itemView =
                LayoutInflater.from(parent.context)
                    .inflate(R.layout.garbage_category_recyclerview_item, parent, false)
            return GarbageCategoriesViewHolder(itemView)
        }

        override fun onBindViewHolder(holder: GarbageCategoriesViewHolder, position: Int) {
            holder.garbageCategoryNameTextView.text = categories[position].name
        }

    override fun getItemCount() = categories.size
}