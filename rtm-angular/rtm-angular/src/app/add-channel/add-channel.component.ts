import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { FormBuilder, FormArray, FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent, MatChipInputEvent } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { ChatService } from '../chat.service';
import { User } from '../User';

@Component({
  selector: 'app-add-channel',
  templateUrl: './add-channel.component.html',
  styleUrls: ['./add-channel.component.css']
})
export class AddChannelComponent implements OnInit {

  visible = true;
  selectable = true;
  removable = true;
  addOnBlur = false;
  separatorKeysCodes: number[] = [ENTER, COMMA];
  userCtrl = new FormControl();
  filteredUsers: Observable<User[]>;
  users: string[] = [];
  workspaceName:string;
  allUsers:User[];

  AddChannelForm = this.fb.group({
  channelName:[''],
  users:this.fb.array([])
  })

  @ViewChild('userInput') userInput: ElementRef<HTMLInputElement>;


  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    // private location: Location,
    private router: Router,
    private chatService: ChatService
  ) {
    this.filteredUsers = this.userCtrl.valueChanges.pipe(
      startWith(null),
      map((user: User | null) => user ? this._filter(user) : this.allUsers.slice())
    )
   }

   add(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;

    // Add our user
    if ((value || '').trim()) {
      this.users.push(value.trim());
    }

    // Reset the input value
    if (input) {
      input.value = '';
    }

    this.userCtrl.setValue(null);
  }

  remove(user: string): void {
    const index = this.users.indexOf(user);

    if (index >= 0) {
      this.users.splice(index, 1);
    }
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    this.users.push(event.option.viewValue);
    this.userInput.nativeElement.value = '';
    this.userCtrl.setValue(null);
  }

  // get users(){
  //   return this.AddChannelForm.get('users') as FormArray
  // }

  ngOnInit() {
  }

  getListOfUsersInWorkspace(){
    console.log("get list of users in workspace");
    this.chatService.getAllUsersInWorkspace(this.workspaceName)
      .subscribe(s => this.allUsers = s);
      console.log(this.allUsers);
  }

  private _filter(value: User): User[] {
    const filterValue = value.firstName.toLowerCase();
    return this.allUsers.filter(x => x.firstName.toLowerCase().indexOf(filterValue) === 0);
  }
}
