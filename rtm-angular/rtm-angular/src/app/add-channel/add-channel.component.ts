import { Component, OnInit, ElementRef, ViewChild, Input } from '@angular/core';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { FormBuilder, FormArray, FormControl, FormGroup } from '@angular/forms';
import { MatAutocompleteSelectedEvent, MatChipInputEvent } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { ChatService } from '../chat.service';
import { User } from '../User';
import { Channel } from '../Channel';
import { Message } from '../Message';

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
  userSelected: User[];
  channelForm: FormGroup;
  currentEmail;
  currentWorkspace;

  public channelToCreate:Channel = {
    "channelId": "",
    "messages": [],
    "workspaceId": "",
    "channelName": "",
    "users": [],
    "admin":{
      "id": "",
      "emailId": "",
      "firstName": "",
      "lastName": "",
      "userId": ""
    }
  };
  allUsers: User[] = [];
  // allUsers: User[] = [
  //   {
  //     "emailId": "a@gmail.com",
  //     "firstName": "Joe",
  //     "lastName": "Doe",
  //     "userId": "12345"
  //   },
  //   {
  //     "emailId": "b@gmail.com",
  //     "firstName": "Joe1",
  //     "lastName": "Doe1",
  //     "userId": "12346"
  //   },
  //   {
  //     "emailId": "c@gmail.com",
  //     "firstName": "Fpp",
  //     "lastName": "Doe",
  //     "userId": "12347"
  //   }
  // ]

  @ViewChild('userInput') userInput: ElementRef<HTMLInputElement>;


  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    // private location: Location,
    private router: Router,
    private chatService: ChatService
  ) {
    // this.filteredUsers = this.userCtrl.valueChanges.pipe(
    //   startWith(null),
    //   map((user: User | null) => user ? this._filter(user) : this.allUsers.slice())
    // )
  }

  // add(event: MatChipInputEvent): void {
  //   const input = event.input;
  //   const value = event.value;

  //   // Add our user
  //   if ((value || '').trim()) {
  //     this.users.push(value.trim());
  //   }

  //   // Reset the input value
  //   if (input) {
  //     input.value = '';
  //   }

  //   this.userCtrl.setValue(null);
  // }

  // remove(user: string): void {
  //   const index = this.users.indexOf(user);

  //   if (index >= 0) {
  //     this.users.splice(index, 1);
  //   }
  // }

  // selected(event: MatAutocompleteSelectedEvent): void {
  //   this.users.push(event.option.viewValue);
  //   this.userInput.nativeElement.value = '';
  //   this.userCtrl.setValue(null);
  // }

  ngOnInit() {
    this.chatService.currentEmailId.subscribe(email => this.currentEmail = email);
    this.chatService.currentWorkspace.subscribe(workspace => this.currentWorkspace = workspace);

    this.channelForm = this.fb.group({
      channelName: new FormControl()
    });
    this.getListOfUsersInWorkspace();
  }

  getListOfUsersInWorkspace() {
    console.log("get list of users in workspace");
    console.log(this.currentWorkspace);
    this.chatService.getAllUsersInWorkspace(this.currentWorkspace)
      .subscribe(s => this.allUsers = s);
  }

  addNewChannel() {
    console.log(this.allUsers);
    console.log("In addNewChannel");
    console.log(this.channelForm.value.channelName);
    for (let user of this.userSelected) {
      let u = user as User;
      this.channelToCreate["users"].push(u);
    }

    this.channelToCreate.channelName = this.channelForm.value.channelName;
    var currentUser = this.allUsers.find(x => x.emailId == this.currentEmail);
    console.log(currentUser);
    console.log(currentUser.emailId);
    this.channelToCreate["users"].push(currentUser);
    this.channelToCreate.admin.id = currentUser.id;
    this.channelToCreate.admin.emailId = currentUser.emailId;
    this.channelToCreate.admin.firstName = currentUser.firstName;
    this.channelToCreate.admin.lastName = currentUser.lastName;
    this.channelToCreate.admin.userId = currentUser.userId;

    this.chatService.createNewChannel(this.channelToCreate, this.currentWorkspace).subscribe();
    console.log(this.channelToCreate);
  }


  private _filter(value: User): User[] {
    const filterValue = value.firstName.toLowerCase();
    return this.allUsers.filter(x => x.firstName.toLowerCase().indexOf(filterValue) === 0);
  }
}
