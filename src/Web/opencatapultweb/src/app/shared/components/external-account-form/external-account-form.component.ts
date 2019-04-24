import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { ExternalAccountTypes } from '@app/core/constants/external-account-types';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-external-account-form',
  templateUrl: './external-account-form.component.html',
  styleUrls: ['./external-account-form.component.css']
})
export class ExternalAccountFormComponent implements OnInit, OnChanges {
  @Input() form: FormGroup;
  @Input() disableForm: boolean;
  @Input() externalAccountIds: { [key: string]: string };
  externalAccountTypes = ExternalAccountTypes;

  externalAccountForm: FormGroup;

  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.externalAccountIds = this.externalAccountIds || {};
    this.externalAccountForm = this.fb.group({});
    for (const externalAccountType of this.externalAccountTypes) {
      this.externalAccountForm.setControl(externalAccountType[0],
        this.fb.control({value: this.externalAccountIds[externalAccountType[0]], disabled: this.disableForm}));
    }

    this.form.setControl('externalAccountIds', this.externalAccountForm);
  }

  ngOnChanges(changes: SimpleChanges) {

    if (changes.disableForm && !changes.disableForm.firstChange) {
      if (this.disableForm) {
        this.externalAccountForm.patchValue(this.externalAccountIds);
        this.externalAccountForm.disable();
      } else {
        this.externalAccountForm.enable();
      }
    }
  }

}
