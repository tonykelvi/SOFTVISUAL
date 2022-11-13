var url = 'http://localhost:3000/'

function cadastrar()
{
	//validacao de alguns dos inputs
	
	
	if(!validaNome('nome'))
	{
		return
	}

    if(!validaEmail('email'))
	{
		return
	}

    if(!validaContato('contato'))
    {
        return
    }
	
	
	//construcao do json que vai no body da criacao de usuario	
	
	let body =
	{
		'nome':        document.getElementById('nome-completo').value,
		'contato':       document.getElementById('contato').value,
		'email':       document.getElementById('email').value,

	};
	
	//envio da requisicao usando a FETCH API
	
	//configuracao e realizacao do POST no endpoint "usuarios"
	fetch(url + "cadastrar/cliente",
	{
		'method': 'POST',
		'redirect': 'follow',
		'headers':
		{
			'Content-Type': 'application/json',
			'Accept': 'application/json'
		},
		'body': JSON.stringify(body)
	})
	//checa se requisicao deu certo
	.then((response) =>
	{
		if(response.ok)
		{
			return response.text()
		}
		else
		{
			return response.text().then((text) =>
			{
				throw new Error(text)
			})
		}
	})
	//trata resposta
	.then((output) =>
	{
		console.log(output)
		alert('Cadastro efetuado! :D')
	})
	//trata erro
	.catch((error) =>
	{
		console.log(error)
		alert('Não foi possível efetuar o cadastro! :(')
	})
	
	//solucao alternativa usando AJAX
	
	/*
	let request = new XMLHttpRequest()
	request.onreadystatechange = () =>
	{
		if(request.readyState === 4)
		{
			if(request.status === 200)
			{
				console.log(request.responseText)
				alert('Cadastro efetuado! :D')
			}
			else
			{
				console.error(request.responseText)
				alert('Não foi possível efetuar o cadastro! :(')
			}
		}
	}
	request.open("POST", url + "usuarios")
	request.setRequestHeader('Accept', 'application/json')
	request.setRequestHeader('Content-type', 'application/json')
	request.send(JSON.stringify(body))
	*/
}

function validaNome(id)
{
	let divNome = document.getElementById(id)
	if(divNome.value.trim().split(' ').length >= 2)
	{
		divNome.style.border = 0
		return true
	}
	else
	{
		divNome.style.border = 'solid 1px red'
		return false
	}
}

function validaEmail(id)
{
    let divEmail = document.getElementById(id)
    let email = divEmail.value

    let temArroba = (/@/).test(email)

    
	if(temArroba)
	{
		divEmail.style.border = 0
		return true
	}
	else
	{
		divEmail.style.border = 'solid 1px red'
		return false
	}

	function validaContato(id)
{
    let divContato = document.getElementById(id)
    let contato = divContato.value

    let temArroba = (/@/).test(contato)

    
	if(temArroba)
	{
		divContato.style.border = 0
		return true
	}
	else
	{
		divContato.style.border = 'solid 1px red'
		return false
	}


}



// function listar()
// {
// 	//da um GET no endpoint "usuarios"
// 	fetch(url)
// 	.then(response => response.json())
// 	.then((cliente) =>
// 	{
// 		//pega div que vai conter a lista de usuarios
// 		let listaClientes = document.getElementById('lista-clientes')
		
// 		//limpa div
// 		while(listaClientes.firstChild)
// 		{
// 			listaClientes.removeChild(listaClientes.firstChild)
// 		}
		
// 		//preenche div com usuarios recebidos do GET
// 		for(let cliente of cliente)
// 		{
// 			//cria div para as informacoes de um usuario
// 			let divCliente = document.createElement('div')
// 			divCliente.setAttribute('class', 'form')
			
// 			//pega o nome do usuario
// 			let divNome = document.createElement('input')
// 			divNome.placeholder = 'nome-cliente'
// 			divNome.value = cliente.nome
// 			divCliente.appendChild(divNome)
			
// 			//pega o email do usuario
// 			let divEmail = document.createElement('input')
// 			divEmail.placeholder = 'Email'
// 			divEmail.value = cliente.email
// 			divCliente.appendChild(divEmail)
			
// 			// //pega o cpf do usuario
// 			// let divCpf = document.createElement('input')
// 			// divCpf.placeholder = 'CPF'
// 			// divCpf.value = usuario.cpf
// 			// divUsuario.appendChild(divCpf)
			
// 			// //cria o botao para remover o usuario
// 			// let btnRemover = document.createElement('button')
// 			// btnRemover.innerHTML = 'Remover'
// 			// btnRemover.onclick = u => remover(usuario.id)
// 			// btnRemover.style.marginRight = '5px'
			
// 			// //cria o botao para atualizar o usuario
// 			// let btnAtualizar = document.createElement('button')
// 			// btnAtualizar.innerHTML = 'Atualizar'
// 			// btnAtualizar.onclick = u => atualizar(usuario.id, divNome, divEmail, divCpf)
// 			// btnAtualizar.style.marginLeft = '5px'
			
// 			// //cria a div com os dois botoes
// 			// let divBotoes = document.createElement('div')
// 			// divBotoes.style.display = 'flex'
// 			// divBotoes.appendChild(btnRemover)
// 			// divBotoes.appendChild(btnAtualizar)
// 			// divUsuario.appendChild(divBotoes)
			
// 			//insere a div do usuario na div com a lista de usuarios
// 			listaClientes.appendChild(divCliente)
// 		}
// 	})
// }

// function atualizar(id, divNome, divEmail, divCpf)
// {
// 	let body =
// 	{
// 		'Nome': divNome.value,
// 		'Email': divEmail.value,
// 		'Cpf': divCpf.value
// 	}
	
// 	fetch(url + "usuarios/" + id,
// 	{
// 		'method': 'PUT',
// 		'redirect': 'follow',
// 		'headers':
// 		{
// 			'Content-Type': 'application/json',
// 			'Accept': 'application/json'
// 		},
// 		'body': JSON.stringify(body)
// 	})
// 	.then((response) =>
// 	{
// 		if(response.ok)
// 		{
// 			return response.text()
// 		}
// 		else
// 		{
// 			return response.text().then((text) =>
// 			{
// 				throw new Error(text)
// 			})
// 		}
// 	})
// 	.then((output) =>
// 	{
// 		listar()
// 		console.log(output)
// 		alert('Usuário atualizado! \\o/')
// 	})
// 	.catch((error) =>
// 	{
// 		console.log(error)
// 		alert('Não foi possível atualizar o usuário :/')
// 	})
// }

// function remover(id)
// {
// 	fetch(url + 'usuarios/' + id,
// 	{
// 		'method': 'DELETE',
// 		'redirect': 'follow'
// 	})
// 	.then((response) =>
// 	{
// 		if(response.ok)
// 		{
// 			return response.text()
// 		}
// 		else
// 		{
// 			return response.text().then((text) =>
// 			{
// 				throw new Error(text)
// 			})
// 		}
// 	})
// 	.then((output) =>
// 	{
// 		listar()
// 		console.log(output)
// 		alert('Usuário removido! >=]')
// 	})
// 	.catch((error) =>
// 	{
// 		console.log(error)
// 		alert('Não foi possível remover o usuário :/')
// 	})
// }
}