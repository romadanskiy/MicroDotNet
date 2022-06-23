import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { FormControl } from '@angular/forms';
import { firstValueFrom, map, Observable, startWith } from 'rxjs';
import { Tag } from '../../models/tag';
import { MatChipInputEvent } from '@angular/material/chips';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { AppService } from '../../services/app.service';

@Component({
  selector: 'app-tags-input',
  templateUrl: './tags-input.component.html',
  styleUrls: ['./tags-input.component.css'],
})
export class TagsInputComponent implements OnInit {
  separatorKeysCodes: number[] = [ENTER, COMMA];
  tagsCtrl = new FormControl('');
  filteredTags?: Observable<Tag[]>;
  @Input() tags: Tag[] = [];
  allTags: Tag[] = [];

  @Output() tagsChange = new EventEmitter<Tag[]>();

  @ViewChild('tagsInput') tagsInput?: ElementRef<HTMLInputElement>;

  constructor(private app: AppService) {
    this.displayFn = this.displayFn.bind(this);
  }

  add(event: MatChipInputEvent): void {
    if (this.tags.length >= 32) return;
    const value = (event.value || '').trim();
    if (value) {
      let tag = this.allTags.find((tag) => tag.text === value);
      if (!tag) tag = { text: value };
      this.tags.push(tag);
      this.tagsChange.emit(this.tags);
    }

    event.chipInput!.clear();
    this.tagsCtrl.setValue('');
  }

  remove(index: number): void {
    if (index >= 0) {
      this.tags.splice(index, 1);
      this.tagsInput!.nativeElement.value = '';
      this.tagsCtrl.setValue('');
      this.tagsChange.emit(this.tags);
    }
  }

  selected(tag: Tag): void {
    if (this.tags.length >= 32) return;
    this.tagsInput!.nativeElement.value = '';
    this.tagsCtrl.setValue('');
    this.tags.push(tag);
    this.tagsChange.emit(this.tags);
  }

  afterSelect(): void {
    this.tagsInput!.nativeElement.value = '';
    this.tagsCtrl.setValue('');
  }

  private _filter(value: string): Tag[] {
    const filterValue = value.toLowerCase();

    return this.allTags.filter((tag) =>
      tag.text.toLowerCase().includes(filterValue),
    );
  }

  ngOnInit(): void {
    this.filteredTags = this.tagsCtrl.valueChanges.pipe(
      startWith(''),
      map((value: string | Tag) =>
        typeof value === 'string' ? value : value?.text,
      ),
      map((text: string) =>
        (text ? this._filter(text) : this.allTags.slice()).filter(
          (tag) => !this.tags.map((t) => t.id).includes(tag.id),
        ),
      ),
    );

    firstValueFrom(this.app.getTags()).then((tags) => {
      this.allTags = tags;
      this.tagsCtrl.setValue(this.tagsCtrl.value);
    });
  }

  displayFn(tag: Tag): string {
    return tag && tag.text ? tag.text : '';
  }
}
